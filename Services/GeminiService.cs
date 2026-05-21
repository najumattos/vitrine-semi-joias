using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.Configurations;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class GeminiService(
    IOptions<GeminiOptions> geminiOptions,
    HttpClient httpClient,
    ILogger<GeminiService> logger) : IGeminiService
{
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";
    private readonly GeminiOptions _config = geminiOptions.Value;

    public async Task<Result<string>> GenerateJewelryDescriptionAsync(Stream fileStream, string mimeType, CancellationToken cancellationToken = default)
    {
        try
        {
            if (fileStream == null || !fileStream.CanRead)
            {
                return Result<string>.Failure("Arquivo de imagem inválido para geração de descrição.");
            }

            if (string.IsNullOrWhiteSpace(mimeType))
            {
                return Result<string>.Failure("Tipo de arquivo inválido.");
            }

            // 1. Prioriza a variável de ambiente (Produção/Azure) e depois o appsettings mapeado no Options
            var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? _config.ApiKey;

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                logger.LogError("Chave da API Gemini não configurada.");
                return Result<string>.Failure("A integração de IA não está configurada no servidor.");
            }

            // 2. Os fallbacks agora podem ser tratados com coalescência nula direta das propriedades tipadas
            var model = string.IsNullOrWhiteSpace(_config.Model) ? "gemini-2.5-flash" : _config.Model;
            var prompt = string.IsNullOrWhiteSpace(_config.Prompt) ? "Recomende uma receita de bolo bem diferente e criativa com passo a passo e modo de preparo" : _config.Prompt;

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

            using var response = await httpClient.PostAsync(endpoint, content, cancellationToken);
            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Erro ao chamar Gemini. Status: {StatusCode}. Resposta: {Response}", response.StatusCode, responseJson);
                return Result<string>.Failure("Não foi possível gerar a descrição no momento. Tente novamente.");
            }

            using var document = JsonDocument.Parse(responseJson);
            var root = document.RootElement;

            if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
            {
                logger.LogWarning("Resposta do Gemini sem candidates. Resposta: {Response}", responseJson);
                return Result<string>.Failure("A IA não retornou uma descrição válida.");
            }

            var description = candidates[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<string>.Failure("A IA não retornou uma descrição válida.");
            }

            return Result<string>.Success(description.Trim());
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Geração de descrição cancelada.");
            return Result<string>.Failure("A geração da descrição foi cancelada.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado ao gerar descrição com Gemini.");
            return Result<string>.Failure("Erro inesperado ao gerar descrição da joia.");
        }
    }
}