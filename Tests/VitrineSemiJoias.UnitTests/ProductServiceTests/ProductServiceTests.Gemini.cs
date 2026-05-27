using Microsoft.AspNetCore.Http;
using NSubstitute;
using VitrineSemiJoias.Common;

namespace VitrineSemiJoias.UnitTests.ProductServiceTests; 

public partial class ProductServiceTests
{
    [Fact]
    public async Task GenerateDescriptionFromImageAsync_DeveRetornarFalha_QuandoArquivoForNulo()
    {
        // Act
        var resultado = await _service.GenerateDescriptionFromImageAsync(null);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("Envie uma imagem válida para gerar a descrição.", resultado.Error);
    }

    [Fact]
    public async Task GenerateDescriptionFromImageAsync_DeveRetornarFalha_QuandoArquivoForVazio()
    {
        // Arrange
        var arquivoMock = Substitute.For<IFormFile>();
        arquivoMock.Length.Returns(0); // Tamanho zero (vazio)

        // Act
        var resultado = await _service.GenerateDescriptionFromImageAsync(arquivoMock);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("Envie uma imagem válida para gerar a descrição.", resultado.Error);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("application/pdf")]
    [InlineData("text/plain")]
    public async Task GenerateDescriptionFromImageAsync_DeveRetornarFalha_QuandoContentTypeNaoForImagem(string? contentTypeInvalido)
    {
        // Arrange
        var arquivoMock = Substitute.For<IFormFile>();
        arquivoMock.Length.Returns(500);
        arquivoMock.ContentType.Returns(contentTypeInvalido!);

        // Act
        var resultado = await _service.GenerateDescriptionFromImageAsync(arquivoMock);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("O arquivo enviado não é uma imagem válida.", resultado.Error);
    }

    [Fact]
    public async Task GenerateDescriptionFromImageAsync_DeveRetornarSucesso_QuandoImagemForValida()
    {
        // Arrange
        var arquivoMock = Substitute.For<IFormFile>();
        arquivoMock.Length.Returns(1024);
        arquivoMock.ContentType.Returns("image/png");
        
        // Simula o stream interno que o xUnit/NSubstitute usará no "OpenReadStream"
        var streamMock = new MemoryStream();
        arquivoMock.OpenReadStream().Returns(streamMock);

        var cts = new CancellationTokenSource();
        var descricaoEsperada = "Um lindo anel de prata cravejado com zircônias.";

        // Mock do GeminiService esperando os parâmetros corretos e o token
        _geminiServiceMock.GenerateJewelryDescriptionAsync(streamMock, "image/png", cts.Token)
            .Returns(Result<string>.Success(descricaoEsperada));

        // Act
        var resultado = await _service.GenerateDescriptionFromImageAsync(arquivoMock, cts.Token);

        // Assert
        Assert.True(resultado.IsSuccess);
        Assert.Equal(descricaoEsperada, resultado.Value);
    }
}