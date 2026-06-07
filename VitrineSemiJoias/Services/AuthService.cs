using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class AuthService(
    UserManager<UserModel> userManager,
    SignInManager<UserModel> signInManager,
    IMapper mapper,
    ILogger<AuthService> logger,
    IEmailSenderService emailSenderService,
    LinkGenerator linkGenerator,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    public async Task Logout(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await signInManager.SignOutAsync();

        logger.LogInformation("Usuário realizou logout e a sessão foi encerrada.");
    }

    public async Task<Result<AuthUserDto>> ValidateUserAsync(LoginDto loginDto)
    {
        if (loginDto == null) return Result<AuthUserDto>.Failure("Dados inválidos.");
        if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
        {
            return Result<AuthUserDto>.Failure("Senha ou email incorretos.");
        }
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            logger.LogWarning("Tentativa de login com e-mail inexistente: {Email}", loginDto.Email);
            return Result<AuthUserDto>.Failure("Senha ou email incorretos.");
        }

        var signInResult = await signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.IsPersistent, lockoutOnFailure: true);
        if (signInResult.IsLockedOut)
        {
            logger.LogWarning("Tentativa de login em conta bloqueada: {Email}", loginDto.Email);
            return Result<AuthUserDto>.Failure("Conta bloqueada temporariamente.");
        }

        if (!signInResult.Succeeded)
        {
            logger.LogWarning("Falha de autenticação (senha incorreta) para o usuário: {Email}", loginDto.Email);
            return Result<AuthUserDto>.Failure("Senha ou email incorretos.");
        }

        var authDto = mapper.Map<AuthUserDto>(user);
        logger.LogInformation("Usuário {Email} autenticado com sucesso.", loginDto.Email);
        return Result<AuthUserDto>.Success(authDto);
    }

    public async Task<Result> ProcessForgotPasswordAsync(ForgotPasswordDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
        {
            return Result.Failure("Dados inválidos.");
        }

        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            logger.LogInformation("Solicitacao de esqueci minha senha para email inexistente: {Email}", dto.Email);
            return Result.Success();
        }

        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            logger.LogWarning("HttpContext indisponivel para gerar link de redefinicao.");
            return Result.Failure("Nao foi possivel processar a solicitacao.");
        }

        try
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var callbackUrl = linkGenerator.GetUriByAction(
                httpContext,
                action: "ResetPassword",
                controller: "Auth",
                values: new { email = user.Email, token = encodedToken });

            if (string.IsNullOrWhiteSpace(callbackUrl))
            {
                logger.LogWarning("Falha ao gerar URL de redefinicao para {Email}", dto.Email);
                return Result.Failure("Nao foi possivel processar a solicitacao.");
            }

            await emailSenderService.SendPasswordResetAsync(user.Email!, callbackUrl, httpContext.RequestAborted);
            logger.LogInformation("Email de redefinicao disparado para {Email}", dto.Email);

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar esqueci minha senha para {Email}", dto.Email);
            return Result.Failure("Nao foi possivel processar a solicitacao.");
        }
    }

    public async Task<Result> ProcessResetPasswordAsync(ResetPasswordDto dto)
    {
        if (dto == null)
        {
            return Result.Failure("Dados inválidos.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Token))
        {
            return Result.Failure("Dados inválidos.");
        }

        if (string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            return Result.Failure("Senha inválida.");
        }

        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            logger.LogWarning("Tentativa de redefinicao com email inexistente: {Email}", dto.Email);
            return Result.Failure("Nao foi possivel redefinir a senha.");
        }

        var decodedToken = Uri.UnescapeDataString(dto.Token);
        var resetResult = await userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);
        if (!resetResult.Succeeded)
        {
            var errors = string.Join("; ", resetResult.Errors.Select(error => error.Description));
            logger.LogWarning("Falha ao redefinir senha para {Email}: {Errors}", dto.Email, errors);
            return Result.Failure(errors);
        }

        logger.LogInformation("Senha redefinida com sucesso para {Email}", dto.Email);
        return Result.Success();
    }

}
