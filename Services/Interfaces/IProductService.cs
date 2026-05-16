
using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Enums;

namespace VitrineSemiJoias.Services.Interfaces;

public interface IProductService
{
    Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync();
    Task<Result<ProductDto>> GetProductByIdAsync(int id);
    Task<Result<IEnumerable<ProductDto>>> GetProductByCategoryAsync(CategoryEnum category);
    Task <Result<ProductDto>> AddProductAsync(ProductDto product, IFormFile arquivoFoto);
    Task<Result<bool>> UpdateProductAsync(ProductDto product, IFormFile arquivoFoto);        
    Task<Result> DeleteProductAsync(ProductDto product);
}
