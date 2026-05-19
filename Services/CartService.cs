using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class CartService(IProductService productService, IHttpContextAccessor httpContextAccessor) : ICartService
{
    private const string SessionKey = "CartItems";

    public async Task<Result> AddItemAsync(int productId)
    {
        if (productId <= 0)
        {
            return Result.Failure("Produto inválido.");
        }

        var context = httpContextAccessor.HttpContext;
        if (context?.Session == null)
        {
            return Result.Failure("A sessão do carrinho não está disponível.");
        }

        var productResult = await productService.GetProductByIdAsync(productId);
        if (!productResult.IsSuccess || productResult.Value == null)
        {
            return Result.Failure(productResult.Error ?? "Produto não encontrado.");
        }

        var product = productResult.Value;
        if (!product.IsAvailable)
        {
            return Result.Failure("Este produto não está disponível para compra.");
        }

        var items = GetSessionItems(context.Session).ToList();
        if (items.Any(item => item.ProductId == product.Id))
        {
            return Result.Failure("Este item já está no carrinho.");
        }

        items.Add(new CartItemDto
        {
            ProductId = product.Id,
            JewelryCode = product.JewelryCode,
            Title = product.Title,
            ImageUrl = product.ImageUrl,
            Price = product.Price
        });

        context.Session.SetJson(SessionKey, items);
        return Result.Success();
    }

    public Task<Result<IReadOnlyCollection<CartItemDto>>> GetItemsAsync()
    {
        var context = httpContextAccessor.HttpContext;
        if (context?.Session == null)
        {
            return Task.FromResult(Result<IReadOnlyCollection<CartItemDto>>.Failure("A sessão do carrinho não está disponível."));
        }

        var items = GetSessionItems(context.Session).ToList().AsReadOnly();
        return Task.FromResult(Result<IReadOnlyCollection<CartItemDto>>.Success(items));
    }

    private static IEnumerable<CartItemDto> GetSessionItems(ISession session)
    {
        return session.GetJson<List<CartItemDto>>(SessionKey) ?? [];
    }
}