using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NSubstitute;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.Configurations;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Services;
using VitrineSemiJoias.Services.Interfaces;

namespace Tests.VitrineSemiJoias.UnitTests;

public class CartServiceTests
{
private readonly IProductService _productServiceMock = Substitute.For<IProductService>();
    private readonly IHttpContextAccessor _httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
    private readonly IOptions<MostruarioOptions> _optionsMock = Substitute.For<IOptions<MostruarioOptions>>();
    private readonly ISession _sessionMock = Substitute.For<ISession>();
    private readonly CartService _service;

    public CartServiceTests()
    {
        // Configura a árvore do HttpContext para retornar o nosso mock de ISession
        var httpContextMock = Substitute.For<HttpContext>();
        httpContextMock.Session.Returns(_sessionMock);
        _httpContextAccessorMock.HttpContext.Returns(httpContextMock);

        // Configuração padrão das opções de mostruário
        _optionsMock.Value.Returns(new MostruarioOptions 
        { 
            WhatsAppNumber = "5514920044824", 
            OwnerName = "Ana Julia" 
        });

        _service = new(_productServiceMock, _httpContextAccessorMock, _optionsMock);
    }

    [Fact]
    public async Task AddItemAsync_DeveRetornarFalha_QuandoProdutoForInvalido()
    {
        // Act
        var resultado = await _service.AddItemAsync(0);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("Produto inválido.", resultado.Error);
    }

    [Fact]
    public async Task AddItemAsync_DeveRetornarFalha_QuandoProdutoNaoForDisponivel()
    {
        // Arrange
        var produtoIndisponivel = new ProductDto { Id = 5, IsAvailable = false };
        _productServiceMock.GetProductByIdAsync(5).Returns(Result<ProductDto>.Success(produtoIndisponivel));

        // Act
        var resultado = await _service.AddItemAsync(5);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("Este produto não está disponível para compra.", resultado.Error);
    }

    [Fact]
    public async Task AddItemAsync_DeveRetornarFalha_QuandoProdutoJaEstiverNoCarrinho()
    {
        // Arrange
        var produto = new ProductDto { Id = 10, IsAvailable = true, Title = "Anel" };
        _productServiceMock.GetProductByIdAsync(10).Returns(Result<ProductDto>.Success(produto));

        // Simulando que a sessão já possui esse item armazenado (Simulação do GetJson interno)
        var itensExistentes = new List<CartItemDto> { new() { ProductId = 10, Title = "Anel" } };
        var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(itensExistentes);
        
        // O método de extensão GetJson chama internamente o TryGetValue da Session
        _sessionMock.TryGetValue("CartItems", out Arg.Any<byte[]>()!)
            .Returns(x => {
                x[1] = jsonBytes; // Preenche o parâmetro out com os bytes do nosso JSON
                return true;
            });

        // Act
        var resultado = await _service.AddItemAsync(10);

        // Assert
        Assert.False(resultado.IsSuccess);
        Assert.Equal("Este item já está no carrinho.", resultado.Error);
    }
}