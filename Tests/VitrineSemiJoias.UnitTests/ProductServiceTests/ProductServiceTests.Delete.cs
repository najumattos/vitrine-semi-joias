using NSubstitute;
using NSubstitute.ExceptionExtensions;
using VitrineSemiJoias.DTOs;

namespace VitrineSemiJoias.UnitTests.ProductServiceTests; 

public partial class ProductServiceTests
{
    [Fact]
    public async Task DeleteProductAsync_DeveRetornarFalha_QuandoProdutoForNulo()
    {
        // Act
        var resultado = await _service.DeleteProductAsync(null);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("Produto inválido ou não informado.", resultado.Error);
    }

    [Fact]
    public async Task DeleteProductAsync_DeveRetornarSucesso_QuandoProdutoForValido()
    {
        // Arrange
        var productDto = new ProductDto 
        { 
            Id = 10, 
            Title = "Colar de Pérolas", 
            ImageUrl = "img/products/colar.jpg" 
        };

        // Métodos que retornam Task (void assíncrono) não precisam de ".Returns()", 
        // o NSubstitute já entende o fluxo de sucesso automaticamente.

        // Act
        var resultado = await _service.DeleteProductAsync(productDto);

        // Assert
        Assert.True(resultado.IsSuccess);

        // Garante que o repositório foi chamado exatamente 1 vez com o ID correto
        await _repositoryMock.Received(1).DeleteProductAsync(10);

        // Garante que o serviço de arquivos foi acionado para limpar o disco
        await _fileServiceMock.Received(1).DeleteFileAsync("img/products/colar.jpg");
    }

    [Fact]
    public async Task DeleteProductAsync_DeveRetornarFalha_QuandoOcorrerExcecaoNoBanco()
    {
        // Arrange
        var productDto = new ProductDto 
        { 
            Id = 20, 
            Title = "Anel de Prata", 
            ImageUrl = "img/products/anel.jpg" 
        };

        // Força o banco de dados a lançar um erro na execução
        _repositoryMock.DeleteProductAsync(productDto.Id).Throws(new Exception("Erro de timeout do banco"));

        // Act
        var resultado = await _service.DeleteProductAsync(productDto);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("Não foi possível excluir o produto.", resultado.Error);

        // GARANTIA DE COMPORTAMENTO: Se o banco quebrou na primeira linha do try, 
        // o código não deve tentar apagar o arquivo para evitar inconsistência.
        await _fileServiceMock.DidNotReceive().DeleteFileAsync(Arg.Any<string>());
    }
}