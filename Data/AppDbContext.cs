using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using VitrineSemiJoias.Models;

namespace VitrineSemiJoias.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opcoes) : DbContext(opcoes)
{
    public DbSet<ProductModel> Products { get; set; }
    public DbSet<UserModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        builder.Entity<UserModel>().HasData(
    new UserModel
    {
        Id = 1,
        Name = "Camila Reis",
        Email = "camila@admin.com",
        PasswordHash = "123456" // Em produção, aqui iria um hash criptografado
    }
);
    }
}
