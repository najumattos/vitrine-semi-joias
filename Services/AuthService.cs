using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class AuthService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IMapper mapper) : IAuthService
{
    public async Task Logout()
    {
        await signInManager.SignOutAsync();
    }

    public async Task<Result<AuthUserDto>> ValidateUserAsync(LoginDto loginDto)
    {
        if (loginDto == null) return Result<AuthUserDto>.Failure("Dados inválidos.");

        var user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
            return Result<AuthUserDto>.Failure("Usuário não encontrado.");

        var signInResult = await signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: loginDto.IsPersistent, lockoutOnFailure: false);
       
        if (!signInResult.Succeeded)
            return Result<AuthUserDto>.Failure("Senha ou email incorretos.");

        var authDto = mapper.Map<AuthUserDto>(user);

        return Result<AuthUserDto>.Success(authDto);
    }
}
