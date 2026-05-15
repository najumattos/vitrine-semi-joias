using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return View(loginVM);
        }

        await SetupAuthenticationCookie(result.Value);
        return RedirectToAction("Index", "Products");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    private async Task SetupAuthenticationCookie(AuthUserDto user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}
