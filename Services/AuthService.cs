using AutoMapper;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;
using VitrineSemiJoias.Repository.Interfaces;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class AuthService(IUserRepository repository, IMapper mapper) : IAuthService
{   
    public async Task<Result<AuthUserDto>> ValidateUserAsync(LoginDto loginDto)
    {
        if (loginDto == null) return Result<AuthUserDto>.Failure("Dados inválidos.");

        var user = await repository.GetUserByEmailAsync(loginDto.Email);

        if (user == null)
            return Result<AuthUserDto>.Failure("Usuário não encontrado.");
      
        // Comparação de senha (lembre-se de evoluir para BCrypt depois!)
        if (user.PasswordHash != loginDto.Password)
            return Result<AuthUserDto>.Failure("Senha ou email incorretos.");

        var authDto = mapper.Map<AuthUserDto>(user);

        return Result<AuthUserDto>.Success(authDto);
    }
}
