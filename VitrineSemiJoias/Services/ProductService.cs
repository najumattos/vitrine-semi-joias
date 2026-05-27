using AutoMapper;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Repository.Interfaces;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class ProductService(
    IProductRepository repository,
    IMapper mapper,
    IFileService fileService,
    IGeminiService geminiService,
    ILogger<ProductService> logger) : IProductService
{
    public async Task<Result<ProductDto>> AddProductAsync(ProductDto product, IFormFile arquivoFoto)
    {
        if (product == null) return Result<ProductDto>.Failure("Produto inválido ou não informado.");
        string? caminhoFotoSalva = null;
        try
        {
            if (arquivoFoto != null && arquivoFoto.Length > 0)
            {
                caminhoFotoSalva = await fileService.SaveFileAsync(arquivoFoto, "img/products");
                product.ImageUrl = caminhoFotoSalva;
            }
            var response = await repository.AddProductAsync(mapper.Map<ProductModel>(product));
            return Result<ProductDto>.Success(mapper.Map<ProductDto>(response));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao tentar adicionar produto no banco de dados.");

            if (!string.IsNullOrEmpty(caminhoFotoSalva))
            {
                await fileService.DeleteFileAsync(caminhoFotoSalva);
            }

            return Result<ProductDto>.Failure("Não foi possível cadastrar o produto no momento.");
        }
    }

    public async Task<Result> DeleteProductAsync(ProductDto product)
    {
        if (product == null)
        {
            return Result.Failure("Produto inválido ou não informado.");
        }
        try
        {
            await repository.DeleteProductAsync(product.Id);
            await fileService.DeleteFileAsync(product.ImageUrl);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao tentar excluir produto ID: {Id}", product.Id);
            return Result.Failure("Não foi possível excluir o produto.");
        }
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        try
        {
            var products = await repository.GetAllProductsAsync();
            var dtos = mapper.Map<IEnumerable<ProductDto>>(products);
            return Result<IEnumerable<ProductDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao listar todos os produtos.");
            return Result<IEnumerable<ProductDto>>.Failure("Não foi possível carregar a lista de produtos.");
        }
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetProductByCategoryAsync(CategoryEnum category)
    {
        if (!Enum.IsDefined(typeof(CategoryEnum), category)) return Result<IEnumerable<ProductDto>>.Failure("Categoria inválida ou não informada.");

        try
        {
            var products = await repository.GetProductByCategoryAsync(category);
            var dtos = mapper.Map<IEnumerable<ProductDto>>(products);
            return Result<IEnumerable<ProductDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar produtos da categoria: {Category}", category);
            return Result<IEnumerable<ProductDto>>.Failure("Não foi possível filtrar os produtos por categoria.");
        }
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(int id)
    {
        if (id <= 0) return Result<ProductDto>.Failure("Produto inválido ou não informado.");

        try
        {
            var product = await repository.GetProductByIdAsync(id);
            if (product == null)
            {
                return Result<ProductDto>.Failure("Produto não encontrado.");
            }
            return Result<ProductDto>.Success(mapper.Map<ProductDto>(product));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar produto pelo ID: {Id}", id);
            return Result<ProductDto>.Failure("Não foi possível obter os detalhes do produto.");
        }
    }

    public async Task<Result<string>> GenerateDescriptionFromImageAsync(IFormFile arquivoFoto, CancellationToken cancellationToken = default)
    {
        if (arquivoFoto == null || arquivoFoto.Length == 0)
        {
            return Result<string>.Failure("Envie uma imagem válida para gerar a descrição.");
        }
        if (string.IsNullOrWhiteSpace(arquivoFoto.ContentType) || !arquivoFoto.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            return Result<string>.Failure("O arquivo enviado não é uma imagem válida.");
        }
        await using var stream = arquivoFoto.OpenReadStream();
        return await geminiService.GenerateJewelryDescriptionAsync(stream, arquivoFoto.ContentType, cancellationToken);
    }

    public async Task<Result<bool>> UpdateProductAsync(ProductDto product, IFormFile arquivoFoto)
    {
        if (product == null) return Result<bool>.Failure("Produto inválido ou não informado.");
        try
        {
            var produtoOriginal = await repository.GetProductByIdAsync(product.Id);
            if (produtoOriginal == null)
            {
                return Result<bool>.Failure("Produto não encontrado no banco de dados.");
            }
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
            await repository.UpdateProductAsync(produtoOriginal);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao tentar atualizar o produto ID: {Id}", product.Id);
            return Result<bool>.Failure("Não foi possível atualizar as informações do produto.");
        }
    }

    private async Task<string> AtualizarFoto(string caminhoAtual, IFormFile novaFoto)
    {
        if (!string.IsNullOrEmpty(caminhoAtual))
        {
            await fileService.DeleteFileAsync(caminhoAtual);
        }
        var novoPath = await fileService.SaveFileAsync(novaFoto, "img/products");
        return novoPath;
    }
}
