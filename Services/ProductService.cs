using AutoMapper;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Repository.Interfaces;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Services;

public class ProductService(IProductRepository repository, IMapper mapper) : IProductService
{
    public async Task<Result<ProductViewModel>> AddProductAsync(ProductViewModel product)
    {
        if (product == null)
        {
            return Result<ProductViewModel>.Failure("Produto inválido ou não informado.");
        }
        var response = await repository.AddProductAsync(mapper.Map<ProductModel>(product));

        if (!response.IsSuccess)
        {
            return Result<ProductViewModel>.Failure(response.Error);
        }
        return Result<ProductViewModel>.Success(mapper.Map<ProductViewModel>(response.Value));

    }

    public async Task<Result> DeleteProductAsync(int id)
    {
        if (id <= 0)
        {
            return Result.Failure("Produto inválido ou não informado.");
        }
        var response = await repository.DeleteProductAsync(id);

        if (!response.IsSuccess)
        {
            return Result.Failure(response.Error);
        }
        return Result.Success();
    }

    public async Task<Result<IEnumerable<ProductViewModel>>> GetAllProductsAsync()
    {
        var response = await repository.GetAllProductsAsync();
        if (!response.IsSuccess)
        {
            return Result<IEnumerable<ProductViewModel>>.Failure(response.Error);
        }
        var productsVM = mapper.Map<IEnumerable<ProductViewModel>>(response.Value);

        return Result<IEnumerable<ProductViewModel>>.Success(productsVM);
    }

    public async Task<Result<ProductViewModel>> GetProductByIdAsync(int id)
    {
        if (id <= 0)
        {
            return Result<ProductViewModel>.Failure("Produto inválido ou não informado.");
        }
        var response = await repository.GetProductByIdAsync(id);

        if (!response.IsSuccess)
        {
            return Result<ProductViewModel>.Failure(response.Error);
        }
        return Result<ProductViewModel>.Success(mapper.Map<ProductViewModel>(response.Value));
    }

    public async Task<Result> UpdateProductAsync(ProductViewModel product)
    {
        if (product == null)
        {
            return Result.Failure("Produto inválido ou não informado.");
        }
        var response = await repository.UpdateProductAsync(mapper.Map<ProductModel>(product));

        if (!response.IsSuccess)
        {
            return Result.Failure(response.Error);
        }
        return Result.Success();
    }  
}
