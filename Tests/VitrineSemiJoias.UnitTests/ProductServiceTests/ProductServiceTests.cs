using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VitrineSemiJoias.Services;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.Repository.Interfaces;

namespace VitrineSemiJoias.UnitTests.ProductServiceTests; 

public partial class ProductServiceTests 
{
    private readonly IProductRepository _repositoryMock = Substitute.For<IProductRepository>();
    private readonly IMapper _mapperMock = Substitute.For<IMapper>();
    private readonly IFileService _fileServiceMock = Substitute.For<IFileService>();
    private readonly IGeminiService _geminiServiceMock = Substitute.For<IGeminiService>();
    private readonly IWebHostEnvironment _environmentMock = Substitute.For<IWebHostEnvironment>();
    private readonly ILogger<ProductService> _loggerMock = Substitute.For<ILogger<ProductService>>();
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _environmentMock.WebRootPath.Returns(Path.Combine(Path.GetTempPath(), "vitrine-semi-joias-tests"));
        _service = new(_repositoryMock, _mapperMock, _fileServiceMock, _geminiServiceMock, _environmentMock, _loggerMock);
    }
}