using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Controllers;

    public class HomeController(IProductService service, IMapper mapper, ILogger<HomeController> logger) : Controller
    {   
        public async Task<IActionResult> Index()
        {
            var result = await service.GetAllProductsAsync();
        if (!result.IsSuccess)
        {            
            logger.LogWarning("Falha ao carregar produtos.");
            return View(Enumerable.Empty<ProductViewModel>());
        }
        var productsVM = mapper.Map<IEnumerable<ProductViewModel>>(result.Value);

        return View(productsVM);    
        }

    [HttpGet]
    public IActionResult Orders() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
