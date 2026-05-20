using System.Text;
using System.Text.Json;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class GeminiService(
	IHttpClientFactory httpClientFactory,
	IConfiguration configuration,
	ILogger<GeminiService> logger) : IGeminiService
{
	private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";

	public async Task<Result<string>> GenerateJewelryDescriptionAsync(Stream fileStream, string mimeType, CancellationToken cancellationToken = default)
	{
		try
		{
			if (fileStream == null || !fileStream.CanRead)
			{
				return Result<string>.Failure("Arquivo de imagem invalido para geracao de descricao.");
			}

			if (string.IsNullOrWhiteSpace(mimeType))
			{
				return Result<string>.Failure("Tipo de arquivo invalido.");
			}

			var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")
				?? configuration["Gemini:ApiKey"];

			if (string.IsNullOrWhiteSpace(apiKey))
			{
				logger.LogError("Chave da API Gemini nao configurada.");
				return Result<string>.Failure("A integracao de IA nao esta configurada no servidor.");
			}

			var model = configuration["Gemini:Model"];
			if (string.IsNullOrWhiteSpace(model))
			{
				model = "gemini-1.5-flash";
			}

			var prompt = configuration["Gemini:Prompt"];
			if (string.IsNullOrWhiteSpace(prompt))
			{
				prompt = "Crie uma descricao comercial curta e objetiva de uma semijoia em portugues do Brasil.";
			}

			using var memoryStream = new MemoryStream();
			await fileStream.CopyToAsync(memoryStream, cancellationToken);
			var base64Image = Convert.ToBase64String(memoryStream.ToArray());

			var requestBody = new
			{
				contents = new[]
				{
					new
					{
						parts = new object[]
						{
							new { text = prompt },
							new
							{
								inline_data = new
								{
									mime_type = mimeType,
									data = base64Image
								}
							}
						}
					}
				}
			};

			var requestJson = JsonSerializer.Serialize(requestBody);
			using var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
			var endpoint = $"{BaseUrl}/{model}:generateContent?key={Uri.EscapeDataString(apiKey)}";

			var client = httpClientFactory.CreateClient();
			using var response = await client.PostAsync(endpoint, content, cancellationToken);
			var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				logger.LogError("Erro ao chamar Gemini. Status: {StatusCode}. Resposta: {Response}", response.StatusCode, responseJson);
				return Result<string>.Failure("Nao foi possivel gerar a descricao no momento. Tente novamente.");
			}

			using var document = JsonDocument.Parse(responseJson);
			var root = document.RootElement;

			if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
			{
				logger.LogWarning("Resposta do Gemini sem candidates. Resposta: {Response}", responseJson);
				return Result<string>.Failure("A IA nao retornou uma descricao valida.");
			}

			var description = candidates[0]
				.GetProperty("content")
				.GetProperty("parts")[0]
				.GetProperty("text")
				.GetString();

			if (string.IsNullOrWhiteSpace(description))
			{
				return Result<string>.Failure("A IA nao retornou uma descricao valida.");
			}

			return Result<string>.Success(description.Trim());
		}
		catch (OperationCanceledException)
		{
			logger.LogWarning("Geracao de descricao cancelada.");
			return Result<string>.Failure("A geracao da descricao foi cancelada.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Erro inesperado ao gerar descricao com Gemini.");
			return Result<string>.Failure("Erro inesperado ao gerar descricao da joia.");
		}
	}

}