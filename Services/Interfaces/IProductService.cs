using VitrineSemiJoias.Common;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Services.Interfaces;

public interface IProductService
{
    Task<Result<IEnumerable<ProductViewModel>>> GetAllProductsAsync();
    Task<Result<ProductViewModel>> GetProductByIdAsync(int id);
    Task <Result<ProductViewModel>> AddProductAsync(ProductViewModel product, IFormFile arquivoFoto);
    Task<Result> UpdateProductAsync(ProductViewModel product, IFormFile arquivoFoto);        
    Task<Result> DeleteProductAsync(int id);
}
