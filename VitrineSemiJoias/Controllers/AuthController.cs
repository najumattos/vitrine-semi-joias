using Microsoft.AspNetCore.Mvc;
using VitrineSemiJoias.Services.Interfaces;
using VitrineSemiJoias.ViewModels;
using AutoMapper;
using VitrineSemiJoias.DTOs;

namespace VitrineSemiJoias.Controllers;

public class AuthController(
    IAuthService service,
    IMapper mapper, 
    ILogger<AuthController> logger) : Controller
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
            ModelState.AddModelError(string.Empty, result.Error ?? "Ocorreu um erro inesperado ao realizar o login.");
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
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var dto = mapper.Map<ForgotPasswordDto>(viewModel);
        var result = await service.ProcessForgotPasswordAsync(dto);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Falha ao processar esqueci minha senha para {Email}: {Error}", viewModel.Email, result.Error);
            ModelState.AddModelError(string.Empty, "Nao foi possivel processar a solicitacao.");
            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Verifique seu e-mail. Se o endereco estiver cadastrado, voce recebera as instrucoes para redefinir sua senha.";
        return RedirectToAction(nameof(ForgotPassword), new { returnUrl });
    }

[HttpGet]
    public IActionResult ResetPassword(string? email = null, string? token = null, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        var viewModel = new ResetPasswordViewModel
        {
            Email = email ?? string.Empty,
            Token = token ?? string.Empty
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var dto = mapper.Map<ResetPasswordDto>(viewModel);
        var result = await service.ProcessResetPasswordAsync(dto);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Falha ao redefinir senha para {Email}: {Error}", viewModel.Email, result.Error);
            ModelState.AddModelError(string.Empty, result.Error ?? "Nao foi possivel redefinir a senha.");
            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Senha redefinida com sucesso. Faca login para continuar.";
        return RedirectToAction(nameof(Login), new { returnUrl });
    }
}
