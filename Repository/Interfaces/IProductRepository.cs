using VitrineSemiJoias.Common;
using VitrineSemiJoias.Models;

namespace VitrineSemiJoias.Repository.Interfaces;

public interface IProductRepository
{
    Task<Result<IEnumerable<ProductModel>>> GetAllProductsAsync();
    Task<Result<ProductModel>> GetProductByIdAsync(int id);
    Task<Result<ProductModel>> AddProductAsync(ProductModel product);
    Task<Result> UpdateProductAsync(ProductModel product);
    Task<Result> DeleteProductAsync(int id);
}
