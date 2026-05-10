using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Update(int id)
    {
        var result = await service.GetProductByIdAsync(id);

        if (!result.IsSuccess)
            return NotFound();

        return View(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProductViewModel product, IFormFile arquivoFoto)
    {
        if (!ModelState.IsValid) return View(product);

        var result = await service.UpdateProductAsync(product, arquivoFoto);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, result.Error);
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await service.DeleteProductAsync(id);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        TempData["Error"] = result.Error;
        return RedirectToAction(nameof(Index));
    }
}
