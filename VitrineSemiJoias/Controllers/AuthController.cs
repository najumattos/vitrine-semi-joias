using Microsoft.AspNetCore.Mvc;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.ViewModels;
using AutoMapper;
using VitrineSemiJoias.DTOs;

namespace VitrineSemiJoias.Controllers;

public class AuthController(IAuthService service, IMapper mapper) : Controller
{
    [HttpGet]
    public IActionResult Login([FromQuery] string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated ?? false)
            return RedirectToAction("Index", "Products");
        
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginVM, [FromQuery] string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(loginVM);

        var loginDto = mapper.Map<LoginDto>(loginVM);
        var result = await service.ValidateUserAsync(loginDto);

        if (!result.IsSuccess)
        {            
            ModelState.AddModelError(string.Empty, result.Error);
            return View(loginVM);
        }     
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Products");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await service.Logout(cancellationToken);
        return RedirectToAction("Login");
    }    

    [HttpGet]
    public IActionResult ForgotPassword(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

   [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult ForgotPassword(ForgotPasswordViewModel viewModel, string? returnUrl = null)
{
    ViewData["ReturnUrl"] = returnUrl;

    if (!ModelState.IsValid)
    {
        return View(viewModel);
    }     
    
    TempData["SuccessMessage"] = "Verifique seu e-mail. Se o endereço estiver cadastrado, você receberá as instruções para redefinir sua senha.";

    return RedirectToAction(nameof(ForgotPassword), new { returnUrl });
}

[HttpGet]
    public IActionResult ResetPassword(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
}
