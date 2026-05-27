using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;

namespace VitrineSemiJoias.Services.Interfaces;

/// <summary>
/// Define os contratos e operações de negócio para o gerenciamento do carrinho de compras temporário do cliente.
/// </summary>
public interface ICartService
{
    /// <summary>
    /// Valida o produto, verifica sua disponibilidade no catálogo e o adiciona ao carrinho armazenado na sessão atual.
    /// </summary>
    /// <param name="productId">O identificador exclusivo (ID) do produto que será adicionado.</param>
    /// <returns>Um objeto Result indicando se o produto foi inserido com sucesso ou a mensagem de falha (ex: produto esgotado ou já inserido).</returns>
    Task<Result> AddItemAsync(int productId);
    
    /// <summary>
    /// Recupera de forma segura todos os itens atualmente armazenados no carrinho do usuário.
    /// </summary>
    /// <returns>Um objeto Result contendo uma coleção imutável de leitura (<see cref="IReadOnlyCollection{CartItemDto}"/>) com os produtos do carrinho.</returns>
    Task<Result<IReadOnlyCollection<CartItemDto>>> GetItemsAsync();
    
    /// <summary>
    /// Orquestra a leitura dos itens do carrinho, calcula os totais e gera uma URL formatada e escapada 
    /// para direcionar o fechamento do pedido diretamente para o WhatsApp do lojista.
    /// </summary>
    /// <returns>Um objeto Result contendo a string da URL do WhatsApp parametrizada com o texto do pedido ou uma mensagem de erro caso o carrinho esteja vazio.</returns>
    Task<Result<string>> GenerateWhatsAppMessageAsync();
}