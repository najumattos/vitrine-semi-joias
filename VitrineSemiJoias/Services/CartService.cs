using System.Globalization;
using System.Text;
using Microsoft.Extensions.Options;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.Configurations;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class CartService(
    IProductService productService,
    IHttpContextAccessor httpContextAccessor,
    IOptions<MostruarioOptions> mostruarioOptions) : ICartService
{
    private const string SessionKey = "CartItems";
private readonly MostruarioOptions _config = mostruarioOptions.Value;
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

    public async Task<Result<string>> GenerateWhatsAppMessageAsync()
    {
        var cartResult = await GetItemsAsync();
        if (!cartResult.IsSuccess || cartResult.Value == null || !cartResult.Value.Any())
        {
            return Result<string>.Failure("Carrinho vazio. Não é possível gerar a mensagem.");
        }
        var culturaBr = new CultureInfo("pt-BR");
        var whatsappNumber = string.IsNullOrWhiteSpace(_config.WhatsAppNumber)
            ? "5514920044824"
            : _config.WhatsAppNumber.Trim();
        var ownerName = string.IsNullOrWhiteSpace(_config.OwnerName)
            ? "Ana Julia"
            : _config.OwnerName.Trim();
        
        var items = cartResult.Value;
        var subtotal = items.Sum(item => item.Price);
        var currentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine($"Olá {ownerName}, aqui está o resumo do pedido montado através do mostruario virtual");
        messageBuilder.AppendLine("-----------------------");
        messageBuilder.AppendLine($"SubTotal: {subtotal.ToString("C", culturaBr)}");
        messageBuilder.AppendLine($"Horario: {currentDateTime}");

        foreach (var item in items)
        {
            messageBuilder.AppendLine("-----------------------");
            messageBuilder.AppendLine($"# {item.JewelryCode} | {item.Title}");
            messageBuilder.AppendLine($"R$ {item.Price.ToString("C", culturaBr)}");
        }

        var messageText = messageBuilder.ToString();
        var escapedMessage = Uri.EscapeDataString(messageText);
        var whatsappUrl = $"https://wa.me/{whatsappNumber}?text={escapedMessage}";

        return Result<string>.Success(whatsappUrl);
    }
}