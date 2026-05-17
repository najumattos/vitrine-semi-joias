using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Data;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Repository.Interfaces;

namespace VitrineSemiJoias.Repository;

public class UserRepository(AppDbContext context) : IUserRepository
{
   
}
