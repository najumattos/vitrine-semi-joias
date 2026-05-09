using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.ViewModels;

namespace VitrineSemiJoias.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opcoes) : DbContext(opcoes)
{
    public DbSet<ProductModel> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

public DbSet<VitrineSemiJoias.ViewModels.ProductViewModel> ProductViewModel { get; set; }
}
