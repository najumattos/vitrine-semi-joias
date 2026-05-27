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
    ILogger<AuthService> logger) : IAuthService
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

}
