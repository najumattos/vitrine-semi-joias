using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;

namespace VitrineSemiJoias.Services.Interfaces;

public interface ICartService
{
    Task<Result> AddItemAsync(int productId);
    Task<Result<IReadOnlyCollection<CartItemDto>>> GetItemsAsync();
    Task<Result<string>> GenerateWhatsAppMessageAsync();
}