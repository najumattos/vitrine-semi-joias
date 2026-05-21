using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Controllers;

public class ProductsController(IProductService service, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string searchTerm)
    {
        var result = await service.GetAllProductsAsync();
        if (!result.IsSuccess)
        {
            ViewData["SearchTerm"] = searchTerm;
            return View(Enumerable.Empty<ProductViewModel>());
        }

        var normalizedSearchTerm = searchTerm?.Trim();
        var products = result.Value ?? Enumerable.Empty<ProductDto>();

        if (!string.IsNullOrWhiteSpace(normalizedSearchTerm))
        {
            products = products.Where(product =>
                product.Title.Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                product.JewelryCode.ToString().Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        ViewData["SearchTerm"] = normalizedSearchTerm;
        return View(mapper.Map<IEnumerable<ProductViewModel>>(products));
    }
     
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await service.GetProductByIdAsync(id);

        if (!result.IsSuccess)
            return NotFound(result.Error); 

        return View(mapper.Map<ProductViewModel>(result.Value));
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

        return View("~/Views/Home/Index.cshtml", mapper.Map<IEnumerable<ProductViewModel>>(result.Value));
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> GenerateDescription(IFormFile arquivoFoto, CancellationToken cancellationToken)
    {
        var result = await service.GenerateDescriptionFromImageAsync(arquivoFoto, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new
            {
                success = false,
                message = result.Error ?? "Não foi possível gerar a descrição da joia."
            });
        }

        return Json(new
        {
            success = true,
            description = result.Value
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel product, IFormFile arquivoFoto)
    {
        if (!ModelState.IsValid) return View(product);
        ;
        var result = await service.AddProductAsync(mapper.Map<ProductDto>(product), arquivoFoto);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index)); 

        ModelState.AddModelError(string.Empty, result.Error);
        return View(mapper.Map<ProductViewModel>(result.Value));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await service.GetProductByIdAsync(id);

        if (!result.IsSuccess)
            return NotFound();
        var productVM = mapper.Map<ProductViewModel>(result.Value);
        return View(productVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductViewModel product, IFormFile arquivoFoto)
    {
        if (!ModelState.IsValid) return View(product);
        var productDto = mapper.Map<ProductDto>(product);
        var result = await service.UpdateProductAsync(productDto, arquivoFoto);

        if (result.IsSuccess){
            return RedirectToAction(nameof(Index));
        }
        

        ModelState.AddModelError(string.Empty, result.Error);
        var productVM = mapper.Map<ProductViewModel>(result);
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        // Busca o produto para exibir os detalhes na tela de confirma��o
        var product = await service.GetProductByIdAsync(id);

        if (product == null){
            return NotFound();
        }

        
        return View(mapper.Map<ProductViewModel>(product.Value));
    }
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(ProductViewModel product)
    {
        var result = await service.DeleteProductAsync(mapper.Map<ProductDto>(product));

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        TempData["Error"] = result.Error;
        return RedirectToAction(nameof(Index));
    }
}
