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
    }
}
