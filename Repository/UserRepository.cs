using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Data;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Repository.Interfaces;

namespace VitrineSemiJoias.Repository;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<UserModel?> GetUserByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
