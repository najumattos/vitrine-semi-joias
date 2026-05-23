using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Models;

namespace VitrineSemiJoias.Repository.Interfaces;

/// <summary>
/// Define o contrato de persistência de dados para a entidade de Produtos no banco de dados.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Obtém todos os produtos cadastrados na vitrine.
    /// </summary>
    /// <returns>Uma coleção contendo todos os produtos encontrados no banco de dados.</returns>
    Task<IEnumerable<ProductModel>> GetAllProductsAsync();
    
    /// <summary>
    /// Busca um produto específico através do seu identificador único.
    /// </summary>
    /// <param name="id">O identificador exclusivo do produto no banco de dados.</param>
    /// <returns>A entidade do produto correspondente ao ID informado, ou nulo caso não seja encontrado.</returns>
    Task<ProductModel> GetProductByIdAsync(int id);
    
    /// <summary>
    /// Filtra e obtém uma lista de produtos baseada em uma categoria específica de semijoia.
    /// </summary>
    /// <param name="category">O enumerador que representa a categoria do produto (ex: Brincos, Anéis, Colares).</param>
    /// <returns>Uma coleção de produtos pertencentes à categoria informada.</returns>
    Task<IEnumerable<ProductModel>> GetProductByCategoryAsync(CategoryEnum category);
    
    /// <summary>
    /// Insere um novo produto no banco de dados da aplicação.
    /// </summary>
    /// <param name="product">A entidade do produto contendo as informações a serem persistidas.</param>
    /// <returns>A entidade do produto cadastrada, preenchida com o ID gerado pelo banco de dados.</returns>
    Task<ProductModel> AddProductAsync(ProductModel product);
    
    /// <summary>
    /// Atualiza as informações de um produto existente no banco de dados.
    /// </summary>
    /// <param name="product">A entidade do produto contendo os dados modificados que serão salvos.</param>
    Task UpdateProductAsync(ProductModel product);

    /// <summary>
    /// Remove um produto do banco de dados através do seu identificador.
    /// </summary>
    /// <param name="id">O identificador exclusivo do produto que será removido.</param>
    Task DeleteProductAsync(int id);
}