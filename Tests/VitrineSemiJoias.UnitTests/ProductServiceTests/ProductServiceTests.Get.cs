using NSubstitute;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Models;

namespace VitrineSemiJoias.UnitTests.ProductServiceTests; 

public partial class ProductServiceTests
{
    #region Testes do GetProductByIdAsync

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetProductByIdAsync_DeveRetornarFalha_QuandoIdForInvalido(int idInvalido)
    {
        // Act
        var resultado = await _service.GetProductByIdAsync(idInvalido);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("Produto inválido ou não informado.", resultado.Error);
    }

    [Fact]
    public async Task GetProductByIdAsync_DeveRetornarFalha_QuandoProdutoNaoExistirNoBanco()
    {
        // Arrange - Força o banco a retornar null para o ID 99
        _repositoryMock.GetProductByIdAsync(99).Returns((ProductModel)null!);

        // Act
        var resultado = await _service.GetProductByIdAsync(99);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("Produto não encontrado.", resultado.Error);
    }

    [Fact]
    public async Task GetProductByIdAsync_DeveRetornarSucesso_QuandoProdutoExistir()
    {
        // Arrange
        var model = new ProductModel { Id = 1, Title = "Anel" };
        var dto = new ProductDto { Id = 1, Title = "Anel" };

        _repositoryMock.GetProductByIdAsync(1).Returns(model);
        _mapperMock.Map<ProductDto>(model).Returns(dto);

        // Act
        var resultado = await _service.GetProductByIdAsync(1);

        // Assert
        Assert.True(resultado.IsSuccess);
        Assert.Equal("Anel", resultado.Value.Title);
    }

    #endregion

    #region Testes do GetProductByCategoryAsync

    [Fact]
    public async Task GetProductByCategoryAsync_DeveRetornarFalha_QuandoCategoriaForInvalida()
    {
        // Arrange - Força um cast inválido de inteiro para o Enum de categoria
        var categoriaInvalida = (CategoryEnum)999;

        // Act
        var resultado = await _service.GetProductByCategoryAsync(categoriaInvalida);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("Categoria inválida ou não informada.", resultado.Error);
    }

    [Fact]
    public async Task GetProductByCategoryAsync_DeveRetornarListaDeProdutos_QuandoCategoriaForValida()
    {
        // Arrange
        var categoria = CategoryEnum.Anel; 
        var listaModels = new List<ProductModel> { new() { Id = 1, Title = "Anel Ouro" } };
        var listaDtos = new List<ProductDto> { new() { Id = 1, Title = "Anel Ouro" } };

        _repositoryMock.GetProductByCategoryAsync(categoria).Returns(listaModels);
        _mapperMock.Map<IEnumerable<ProductDto>>(listaModels).Returns(listaDtos);

        // Act
        var resultado = await _service.GetProductByCategoryAsync(categoria);

        // Assert
        Assert.True(resultado.IsSuccess);
        Assert.Single(resultado.Value); // Verifica se veio exatamente 1 item na lista
    }

    #endregion
}