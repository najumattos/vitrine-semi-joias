using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using VitrineSemiJoias.Data;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private const string InMemoryDatabaseName = "IntegrationTestsDb";

    public IEmailSenderService EmailSenderMock { get; } = Substitute.For<IEmailSenderService>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                d.ServiceType == typeof(DbContextOptions)).ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            var internalServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            services.AddDbContext<AppDbContext>((_, options) =>
            {
                options.UseInMemoryDatabase(InMemoryDatabaseName)
                       .UseInternalServiceProvider(internalServiceProvider);
            });

            var emailDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailSenderService));
            if (emailDescriptor != null)
            {
                services.Remove(emailDescriptor);
            }

            services.AddSingleton(EmailSenderMock);

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
            });
        });

        builder.ConfigureTestServices(services =>
        {
            foreach (var descriptor in services.Where(d => d.ServiceType == typeof(IAntiforgery)).ToList())
            {
                services.Remove(descriptor);
            }

            services.AddSingleton<IAntiforgery, AlwaysValidAntiforgery>();
        });
    }
}
