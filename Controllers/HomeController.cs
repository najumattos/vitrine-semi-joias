using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Controllers;

    public class HomeController(IProductService service, ICartService cartService, IMapper mapper, ILogger<HomeController> logger) : Controller
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
    public async Task<IActionResult> Orders()
    {
        var cartResult = await cartService.GetItemsAsync();

        if (!cartResult.IsSuccess)
        {
            TempData["ErrorMessage"] = cartResult.Error;
            return View(new OrdersViewModel());
        }

        var viewModel = new OrdersViewModel
        {
            Items = mapper.Map<IReadOnlyCollection<CartItemViewModel>>(cartResult.Value)
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int productId)
    {
        var result = await cartService.AddItemAsync(productId);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Produto adicionado ao carrinho.";
        }
        else
        {
            TempData["ErrorMessage"] = result.Error;
        }

        return RedirectToAction(nameof(Orders));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FinalizeOrder()
    {
        var messageResult = await cartService.GenerateWhatsAppMessageAsync();

        if (!messageResult.IsSuccess || string.IsNullOrWhiteSpace(messageResult.Value))
        {
            TempData["ErrorMessage"] = messageResult.Error ?? "Erro ao gerar o link do WhatsApp.";
            return RedirectToAction(nameof(Orders));
        }

        return Redirect(messageResult.Value);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
