using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VitrineSemiJoias.Data;

namespace VitrineSemiJoias.IntegrationTests;

// O 'Program' aqui se refere à classe do seu Program.cs do projeto web
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
    {
        // 1. Remove a configuração original do SQL Server
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        // 2. Adiciona o EF Core In-Memory Database
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("BancoDeTestesIntegrados");
        });

        // 🔥 3. Força o ASP.NET Core a ignorar o ValidateAntiForgeryToken nos testes
        services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute());
        });

        // 4. Garante a criação do esquema do banco em memória
        var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    });
    }
}