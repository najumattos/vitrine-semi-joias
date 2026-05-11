using AutoMapper;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Repository.Interfaces;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Services;

public class ProductService(IProductRepository repository, IMapper mapper, IFileService fileService) : IProductService
{
    public async Task<Result<ProductViewModel>> AddProductAsync(ProductViewModel product, IFormFile arquivoFoto)
    {
        if (product == null)
        {
            return Result<ProductViewModel>.Failure("Produto inválido ou não informado.");
        }
        if (arquivoFoto != null)
        {
            product.ImageUrl = await fileService.SaveFileAsync(arquivoFoto, "img/products");
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

    public async Task<Result<IEnumerable<ProductViewModel>>> GetProductByCategoryAsync(CategoryEnum category)
    {
        if (!Enum.IsDefined(typeof(CategoryEnum), category))
        {
            return Result<IEnumerable<ProductViewModel>>.Failure("Categoria inválida ou não informada.");
        }
        var response = await repository.GetProductByCategoryAsync(category);

        if (!response.IsSuccess)
        {
            return Result<IEnumerable<ProductViewModel>>.Failure(response.Error);
        }
        return Result<IEnumerable<ProductViewModel>>.Success(mapper.Map<IEnumerable<ProductViewModel>>(response.Value));
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

    public async Task<Result> UpdateProductAsync(ProductViewModel product, IFormFile arquivoFoto)
    {
        if (product == null)
        {
            return Result.Failure("Produto inválido ou não informado.");
        }
        if (arquivoFoto != null)
        {
            await AtualizarFoto(product, arquivoFoto);
        }
        var response = await repository.UpdateProductAsync(mapper.Map<ProductModel>(product));

        if (!response.IsSuccess)
        {
            return Result.Failure(response.Error);
        }
        return Result.Success();
    }

    private async Task<string> AtualizarFoto(ProductViewModel product, IFormFile novaFoto)
    {              
        //Se o usuário já tiver uma foto, a antiga é deletada
        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            await fileService.DeleteFileAsync(product.ImageUrl);
        }

        var novoPath = await fileService.SaveFileAsync(novaFoto, "img/products");

        // 3. Atualiza o caminho da string no banco de dados
        product.ImageUrl = novoPath;

        // 4. Retornamos a URL completa para o Front-end já exibir a imagem
        return fileService.GetFileUrl(novoPath);
    }
}
