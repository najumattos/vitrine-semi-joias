using Microsoft.AspNetCore.Mvc;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Controllers;

public class ProductsController(IProductService service) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await service.GetAllProductsAsync();

        if (!result.IsSuccess)
            return View(Enumerable.Empty<ProductViewModel>());

        return View(result.Value);
    }
     
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await service.GetProductByIdAsync(id);

        if (!result.IsSuccess)
            return NotFound(result.Error); 

        return View(result.Value);
    }
    [HttpGet]
    public async Task<IActionResult> Category(CategoryEnum? category)
    {
        if (!category.HasValue || !Enum.IsDefined(typeof(CategoryEnum), category.Value))
        {
            var allProducts = await service.GetAllProductsAsync();
            return View("~/Views/Home/Index.cshtml", allProducts.IsSuccess ? allProducts.Value : Enumerable.Empty<ProductViewModel>());
        }
        var result = await service.GetProductByCategoryAsync(category.Value);

        if (!result.IsSuccess)
        {
            // Opcional: Enviar mensagem de erro via TempData ou ViewBag
            TempData["ErrorMessage"] = result.Error;
            return View("~/Views/Home/Index.cshtml", Enumerable.Empty<ProductViewModel>());
        }

        return View("~/Views/Home/Index.cshtml", result.Value);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel product, IFormFile arquivoFoto)
    {
        if (!ModelState.IsValid) return View(product);

        var result = await service.AddProductAsync(product, arquivoFoto);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index)); 

        ModelState.AddModelError(string.Empty, result.Error);
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await service.GetProductByIdAsync(id);

        if (!result.IsSuccess)
            return NotFound();

        return View(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductViewModel product, IFormFile arquivoFoto)
    {
        if (!ModelState.IsValid) return View(product);

        var result = await service.UpdateProductAsync(product, arquivoFoto);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, result.Error);
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        // Busca o produto para exibir os detalhes na tela de confirmação
        var product = await service.GetProductByIdAsync(id);

        if (product == null){
            return NotFound();
        }
        
        ProductViewModel model = product.Value;
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await service.DeleteProductAsync(id);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        TempData["Error"] = result.Error;
        return RedirectToAction(nameof(Index));
    }
}
