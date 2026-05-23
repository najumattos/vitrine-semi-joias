using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.ViewModels;
using VitrineSemiJoias.DTOs;

namespace VitrineSemiJoias.Controllers;

    public class HomeController(
        IProductService service, 
        ICartService cartService,
        IMapper mapper, 
        ILogger<HomeController> logger) : Controller
    {   
        public async Task<IActionResult> Index(string searchTerm, CategoryEnum? category)
        {
            var result = await service.GetAllProductsAsync();
        if (!result.IsSuccess)
        {            
            logger.LogWarning("Falha ao carregar produtos.");
                ViewData["SearchTerm"] = searchTerm;
                ViewData["SelectedCategory"] = category?.ToString();
            return View(Enumerable.Empty<ProductViewModel>());
        }

            var normalizedSearchTerm = searchTerm?.Trim();
            var products = result.Value ?? Enumerable.Empty<ProductDto>();

            if (category.HasValue && Enum.IsDefined(typeof(CategoryEnum), category.Value))
            {
                products = products.Where(product => product.CategoryEnum == category.Value);
            }

            if (!string.IsNullOrWhiteSpace(normalizedSearchTerm))
            {
                products = products.Where(product =>
                    product.Title.Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    product.JewelryCode.ToString().Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            ViewData["SearchTerm"] = normalizedSearchTerm;
            ViewData["SelectedCategory"] = category?.ToString();
            return View(mapper.Map<IEnumerable<ProductViewModel>>(products));    
        }

    [HttpGet]
    public async Task<IActionResult> Orders()
    {
        var cartResult = await cartService.GetItemsAsync();

        if (!cartResult.IsSuccess)
        {
            TempData["ErrorMessage"] = cartResult.Error;
            return View(new OrderViewModel());
        }

        var viewModel = new OrderViewModel
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
            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Produto adicionado ao pedido" });
            }

            TempData["SuccessMessage"] = "Produto adicionado ao carrinho.";
        }
        else
        {
            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                return BadRequest(new { success = false, message = result.Error ?? "Não foi possível adicionar o produto ao pedido." });
            }

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
