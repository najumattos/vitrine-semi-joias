using VitrineSemiJoias.Common;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Services.Interfaces;

public interface IProductService
{
    Task<Result<IEnumerable<ProductViewModel>>> GetAllProductsAsync();
    Task<Result<ProductViewModel>> GetProductByIdAsync(int id);
    Task<Result<IEnumerable<ProductViewModel>>> GetProductByCategoryAsync(CategoryEnum category);
    Task <Result<ProductViewModel>> AddProductAsync(ProductViewModel product, IFormFile arquivoFoto);
    Task<Result> UpdateProductAsync(ProductViewModel product, IFormFile arquivoFoto);        
    Task<Result> DeleteProductAsync(int id);
}
