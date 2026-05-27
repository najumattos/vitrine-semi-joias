
using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Enums;

namespace VitrineSemiJoias.Services.Interfaces;

/// <summary>
/// Define as regras de negócio e as operações orquestradas para o gerenciamento de produtos da vitrine.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Recupera todos os produtos cadastrados no sistema adaptados para exibição.
    /// </summary>
    /// <returns>Um objeto Result contendo a coleção de DTOs de todos os produtos ou a mensagem de falha.</returns>
    Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync();
    /// <summary>
    /// Obtém os detalhes de um produto específico com base no seu identificador exclusivo.
    /// </summary>
    /// <param name="id">O ID do produto que se deseja buscar.</param>
    /// <returns>Um objeto Result contendo o DTO do produto encontrado ou uma mensagem de erro indicando que não foi localizado.</returns>
        Task<Result<ProductDto>> GetProductByIdAsync(int id);
    
    /// <summary>
    /// Filtra a coleção de produtos ativos na vitrine por meio de sua categoria.
    /// </summary>
    /// <param name="category">O enumerador representando o tipo da semijoia (ex: Brincos, Anéis).</param>
    /// <returns>Um objeto Result contendo a lista de produtos pertencentes à categoria informada.</returns>
    Task<Result<IEnumerable<ProductDto>>> GetProductByCategoryAsync(CategoryEnum category);
    
    /// <summary>
    /// Executa as validações de negócio, realiza o upload da imagem e insere o novo produto no catálogo.
    /// </summary>
    /// <param name="product">O DTO contendo os dados iniciais do produto a ser cadastrado.</param>
    /// <param name="arquivoFoto">O arquivo físico da imagem enviado através do formulário HTTP.</param>
    /// <returns>Um objeto Result contendo o DTO do produto persistido preenchido com suas informações finais.</returns>
    Task <Result<ProductDto>> AddProductAsync(ProductDto product, IFormFile arquivoFoto);
    
    /// <summary>
    /// Envia a imagem de uma semijoia para a API do Google Gemini para inteligência artificial gerar uma descrição comercial descritiva.
    /// </summary>
    /// <param name="arquivoFoto">O arquivo de imagem da semijoia a ser analisado pela IA.</param>
    /// <param name="cancellationToken">Token para cancelamento ou interrupção antecipada da requisição assíncrona.</param>
    /// <returns>Um objeto Result contendo o texto da descrição gerada pela inteligência artificial.</returns>
    Task<Result<string>> GenerateDescriptionFromImageAsync(IFormFile arquivoFoto, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Atualiza as propriedades e/ou substitui a foto de uma semijoia existente no banco de dados.
    /// </summary>
    /// <param name="product">O DTO contendo as modificações solicitadas e o ID original do registro.</param>
    /// <param name="arquivoFoto">O novo arquivo de imagem caso o usuário queira substituir a foto atual (opcional).</param>
    /// <returns>Um objeto Result contendo um booleano que confirma o sucesso da alteração ou os erros mapeados.</returns>
    Task<Result<bool>> UpdateProductAsync(ProductDto product, IFormFile arquivoFoto);        
    
    /// <summary>
    /// Remove de forma consistente o produto do banco de dados e limpa seu arquivo físico de imagem do servidor de arquivos.
    /// </summary>
    /// <param name="product">O DTO contendo os dados identificadores do produto que será excluído.</param>
    /// <returns>Um objeto Result genérico indicando o sucesso ou a mensagem de falha da operação de deleção.</returns>
    Task<Result> DeleteProductAsync(ProductDto product);
}
