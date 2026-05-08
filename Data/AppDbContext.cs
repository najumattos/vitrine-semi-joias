using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Models;

namespace VitrineSemiJoias.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opcoes) : DbContext(opcoes)
{
    public DbSet<ProductModel> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
