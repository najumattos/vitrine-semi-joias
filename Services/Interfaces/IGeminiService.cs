using VitrineSemiJoias.Common;

namespace VitrineSemiJoias.Services.Interfaces;

public interface IGeminiService
{
	Task<Result<string>> GenerateJewelryDescriptionAsync(Stream fileStream, string mimeType, CancellationToken cancellationToken = default);
}