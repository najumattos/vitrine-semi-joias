using VitrineSemiJoias.Common;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Services.Interfaces;

public interface IProductService
{
    Task<Result<IEnumerable<ProductViewModel>>> GetAllProductsAsync();
    Task<Result<ProductViewModel>> GetProductByIdAsync(int id);
    Task <Result<ProductViewModel>> AddProductAsync(ProductViewModel product);
    Task<Result> UpdateProductAsync(ProductViewModel product);        
    Task<Result> DeleteProductAsync(int id);
}
