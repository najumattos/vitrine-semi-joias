using VitrineSemiJoias.Models;

namespace VitrineSemiJoias.Repository.Interfaces;

public interface IUserRepository
{
    Task<UserModel?> GetUserByEmailAsync(string email);
}
