using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;

namespace VitrineSemiJoias.Services.Interfaces;

public interface IAuthService
{
    Task<Result<AuthUserDto>> ValidateUserAsync(LoginDto loginDto);
    Task Logout();
}
