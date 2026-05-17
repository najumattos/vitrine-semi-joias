using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Models;

namespace VitrineSemiJoias.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserModel, IdentityRole<int>, int>(options)
{
    public DbSet<ProductModel> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        var passwordHasher = new PasswordHasher<UserModel>();

        var adminUser = new UserModel
        {
            Id = 1,
            Name = "Camila Reis",
            UserName = "camila@admin.com", 
            Email = "camila@admin.com",
            NormalizedUserName = "CAMILA@ADMIN.COM",
            NormalizedEmail = "CAMILA@ADMIN.COM",   
            SecurityStamp = Guid.NewGuid().ToString() 
        };

        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "123456");
        builder.Entity<UserModel>().HasData(adminUser);
    }
}
