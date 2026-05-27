using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Models;

namespace VitrineSemiJoias.UnitTests.ProductServiceTests; 

public partial class ProductServiceTests 
{
    [Fact]
    public async Task AddProductAsync_DeveRetornarSucesso_QuandoDadosEImagemForemValidos()
    {
        var productDto = new ProductDto { Id = 1, Title = "Anel de Ouro" };
        var productModel = new ProductModel { Id = 1, Title = "Anel de Ouro" };
        
        var arquivoMock = Substitute.For<IFormFile>();
        arquivoMock.Length.Returns(100);

        _fileServiceMock.SaveFileAsync(arquivoMock, "img/products").Returns("caminho/foto.jpg");
        _mapperMock.Map<ProductModel>(productDto).Returns(productModel);
        _repositoryMock.AddProductAsync(productModel).Returns(productModel);
        _mapperMock.Map<ProductDto>(productModel).Returns(productDto);

        var resultado = await _service.AddProductAsync(productDto, arquivoMock);

        Assert.True(resultado.IsSuccess);
        Assert.Equal("caminho/foto.jpg", productDto.ImageUrl);
        Assert.Equal("Anel de Ouro", resultado.Value.Title);
    }

    [Fact]
    public async Task AddProductAsync_DeveApagarFotoERetornarFalha_QuandoOcorrerExcecaoNoBanco()
    {
        var productDto = new ProductDto { Id = 1, Title = "Anel Quebrado" };
        var productModel = new ProductModel { Id = 1, Title = "Anel Quebrado" };
        
        var arquivoMock = Substitute.For<IFormFile>();
        arquivoMock.Length.Returns(100);

        _fileServiceMock.SaveFileAsync(arquivoMock, "img/products").Returns("caminho/foto-deletar.jpg");
        _mapperMock.Map<ProductModel>(productDto).Returns(productModel);
        
        _repositoryMock.AddProductAsync(productModel).Throws(new Exception("Erro de conexão com o banco"));

        var resultado = await _service.AddProductAsync(productDto, arquivoMock);

        Assert.False(resultado.IsSuccess);
        Assert.Equal("Não foi possível cadastrar o produto no momento.", resultado.Error);
        
        await _fileServiceMock.Received(1).DeleteFileAsync("caminho/foto-deletar.jpg");
    }

    [Fact]
    public async Task AddProductAsync_DeveRetornarFalha_QuandoProdutoForNulo()
    {
        var resultado = await _service.AddProductAsync(null, null);

        Assert.False(resultado.IsSuccess);
        Assert.Equal("Produto inválido ou não informado.", resultado.Error);
    }

    [Fact]
    public async Task AddProductAsync_DeveRetornarSucesso_QuandoNaoForEnviadaFoto()
    {
        var productDto = new ProductDto { Id = 2, Title = "Brinco de Prata", ImageUrl = null };
        var productModel = new ProductModel { Id = 2, Title = "Brinco de Prata", ImageUrl = null };

        _mapperMock.Map<ProductModel>(productDto).Returns(productModel);
        _repositoryMock.AddProductAsync(productModel).Returns(productModel);
        _mapperMock.Map<ProductDto>(productModel).Returns(productDto);

        var resultado = await _service.AddProductAsync(productDto, null);

        Assert.True(resultado.IsSuccess);
        Assert.Null(resultado.Value.ImageUrl);
        
        await _fileServiceMock.DidNotReceive().SaveFileAsync(Arg.Any<IFormFile>(), Arg.Any<string>());
    }
}