using Microsoft.AspNetCore.Mvc;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.ViewModels;
using AutoMapper;
using VitrineSemiJoias.DTOs;

namespace VitrineSemiJoias.Controllers;

public class AuthController(IAuthService service, IMapper mapper) : Controller
{
    // GET: AuthController
    [HttpGet]
    public IActionResult Login()
    {
        // Se o usuário já estiver autenticado, redireciona para a Home
        if (User.Identity?.IsAuthenticated ?? false)
            return RedirectToAction("Index", "Products");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginVM)
    {
        if (!ModelState.IsValid) return View(loginVM);

        var loginDto = mapper.Map<LoginDto>(loginVM);
        var result = await service.ValidateUserAsync(loginDto);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Error);
            return View(loginVM);
        }     
        return RedirectToAction("Index", "Products");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await service.Logout();
        return RedirectToAction("Login");
    }    
}
