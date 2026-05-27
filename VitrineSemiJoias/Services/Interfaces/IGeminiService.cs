using VitrineSemiJoias.Common;

namespace VitrineSemiJoias.Services.Interfaces;

/// <summary>
/// Define os contratos para integração com a API de Inteligência Artificial do Google Gemini.
/// </summary>
public interface IGeminiService
{
	/// <summary>
    /// Analisa o fluxo binário de uma imagem de semijoia e gera uma descrição comercial técnica e atrativa por IA.
    /// </summary>
    /// <param name="fileStream">O fluxo de dados (Stream) contendo os bytes lidos da imagem.</param>
    /// <param name="mimeType">O tipo de mídia do arquivo (ex: "image/jpeg", "image/png").</param>
    /// <param name="cancellationToken">Token para monitorar e permitir o cancelamento antecipado da requisição assíncrona.</param>
    /// <returns>Um objeto Result contendo a string da descrição textual gerada pela IA ou a mensagem de falha da integração.</returns>
	Task<Result<string>> GenerateJewelryDescriptionAsync(Stream fileStream, string mimeType, CancellationToken cancellationToken = default);
}