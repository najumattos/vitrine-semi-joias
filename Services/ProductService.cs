using AutoMapper;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Repository.Interfaces;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class ProductService(IProductRepository repository, IMapper mapper, IFileService fileService) : IProductService
{
    public async Task<Result<ProductDto>> AddProductAsync(ProductDto product, IFormFile arquivoFoto)
    {
        if (product == null)
        {
            return Result<ProductDto>.Failure("Produto inválido ou não informado.");
        }
        if (arquivoFoto != null)
        {
            product.ImageUrl = await fileService.SaveFileAsync(arquivoFoto, "img/products");
        }
        var response = await repository.AddProductAsync(mapper.Map<ProductModel>(product));

        if (!response.IsSuccess)
        {
            return Result<ProductDto>.Failure(response.Error);
        }
        return Result<ProductDto>.Success(mapper.Map<ProductDto>(response.Value));

    }

    public async Task<Result> DeleteProductAsync(ProductDto product)
    {
        if (product == null)                     
        {
            return Result.Failure("Produto inválido ou não informado.");
        }
        
        var response = await repository.DeleteProductAsync(product.Id);
        await fileService.DeleteFileAsync(product.ImageUrl);
        if (!response.IsSuccess)
        {
            return Result.Failure(response.Error);
        }
        return Result.Success();
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        var response = await repository.GetAllProductsAsync();
        if (!response.IsSuccess)
        {
            return Result<IEnumerable<ProductDto>>.Failure(response.Error);
        }

        return Result<IEnumerable<ProductDto>>.Success(mapper.Map<IEnumerable<ProductDto>>(response.Value));
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetProductByCategoryAsync(CategoryEnum category)
    {
        if (!Enum.IsDefined(typeof(CategoryEnum), category))
        {
            return Result<IEnumerable<ProductDto>>.Failure("Categoria inválida ou não informada.");
        }
        var response = await repository.GetProductByCategoryAsync(category);

        if (!response.IsSuccess)
        {
            return Result<IEnumerable<ProductDto>>.Failure(response.Error);
        }
        return Result<IEnumerable<ProductDto>>.Success(mapper.Map<IEnumerable<ProductDto>>(response.Value));
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(int id)
    {
        if (id <= 0)
        {
            return Result<ProductDto>.Failure("Produto inválido ou não informado.");
        }
        var response = await repository.GetProductByIdAsync(id);

        if (!response.IsSuccess)
        {
            return Result<ProductDto>.Failure(response.Error);
        }
        return Result<ProductDto>.Success(mapper.Map<ProductDto>(response.Value));
    }

    public async Task<Result<bool>> UpdateProductAsync(ProductDto product, IFormFile arquivoFoto)
    {
        if (product == null)
        {
            return Result<bool>.Failure("Produto inválido ou não informado.");
        }
        var produtoBancoResult = await repository.GetProductByIdAsync(product.Id);
        if (!produtoBancoResult.IsSuccess || produtoBancoResult.Value == null)
        {
            return Result<bool>.Failure("Produto não encontrado no banco de dados.");
        }
        var produtoOriginal = produtoBancoResult.Value;
        produtoOriginal.JewelryCode = product.JewelryCode;
        produtoOriginal.Title = product.Title;
        produtoOriginal.Description = product.Description;
        produtoOriginal.Price = product.Price;       
        produtoOriginal.CategoryEnum = product.CategoryEnum;
        produtoOriginal.IsAvailable = product.IsAvailable;

        if (arquivoFoto != null && arquivoFoto.Length > 0)
        {
            var novaImagem = await AtualizarFoto(produtoOriginal.ImageUrl, arquivoFoto);
            produtoOriginal.ImageUrl = novaImagem;
        }
        var response = await repository.UpdateProductAsync(produtoOriginal);
        if (!response.IsSuccess)
        {
            return Result<bool>.Failure(response.Error);
        }

        return Result<bool>.Success(true);

    }

    private async Task<string> AtualizarFoto(string caminhoAtual, IFormFile novaFoto)
    {
        // Se houver uma foto antiga, tenta removê-la antes de salvar a nova
        if (!string.IsNullOrEmpty(caminhoAtual))
        {
            await fileService.DeleteFileAsync(caminhoAtual);
        }

        var novoPath = await fileService.SaveFileAsync(novaFoto, "img/products");

        return novoPath;
    }
}
