using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.IntegrationTests;

public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IEmailSenderService _emailSender;
    private const string SeedEmail = "camila@admin.com";
    private const string SeedPassword = "SenhaValida123!";
    private string _antiForgeryToken = string.Empty;
    private IEnumerable<string> _securityCookies = Enumerable.Empty<string>();
    private string _callbackUrl = string.Empty;

    public AuthIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _emailSender = Substitute.For<IEmailSenderService>();
        _emailSender
            .SendPasswordResetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _emailSender
            .When(call => call.SendPasswordResetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>()))
            .Do(call => _callbackUrl = call.ArgAt<string>(1));

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailSenderService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddSingleton(_emailSender);
            });
        });

        _client = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    // 🚀 Executa uma única vez ANTES de rodar os testes da classe
    public async Task InitializeAsync()
    {
        var response = await _client.GetAsync("/Auth/Login");
        var html = await response.Content.ReadAsStringAsync();

        var match = Regex.Match(html, @"__RequestVerificationToken"" type=""hidden"" value=""([^""]+)""");
        if (!match.Success)
            throw new InvalidOperationException("Não foi possível localizar o AntiforgeryToken.");

        _antiForgeryToken = match.Groups[1].Value;
        
        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            _securityCookies = cookies;
        }

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
        var existingUser = await userManager.FindByEmailAsync(SeedEmail);
        if (existingUser != null)
        {
            await userManager.DeleteAsync(existingUser);
        }

        var user = new UserModel
        {
            UserName = SeedEmail,
            Email = SeedEmail,
            Name = "Camila Admin",
            Profile = ProfileEnum.Admin
        };

        await userManager.CreateAsync(user, SeedPassword);
    }

    // Executa APÓS o fim de todos os testes (descarte/limpeza se necessário)
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Login_DeveAutenticarERedirecionar_QuandoCredenciaisForemValidas()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/Auth/Login");
        
        foreach (var cookie in _securityCookies)
        {
            request.Headers.TryAddWithoutValidation("Cookie", cookie);
        }

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "__RequestVerificationToken", _antiForgeryToken }, 
            { "Email", SeedEmail },
            { "Password", SeedPassword }
        });

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Products", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task Login_DeveRetornarMesmaViewComErro_QuandoEmailNaoExistir()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/Auth/Login");
        
        foreach (var cookie in _securityCookies) 
            request.Headers.TryAddWithoutValidation("Cookie", cookie);

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "__RequestVerificationToken", _antiForgeryToken },
            { "Email", "email-fantasma@naoexiste.com" },
            { "Password", "qualquer-senha" }
        });

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); 
        
        var htmlContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("Senha ou email incorretos.", htmlContent); 
    }

    [Fact]
    public async Task Login_DeveRetornarMesmaViewComErro_QuandoSenhaForIncorreta()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/Auth/Login");
        
        foreach (var cookie in _securityCookies) 
            request.Headers.TryAddWithoutValidation("Cookie", cookie);

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "__RequestVerificationToken", _antiForgeryToken },
            { "Email", SeedEmail },
            { "Password", "SenhaErrada123!" }
        });

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); 
        
        var htmlContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("Senha ou email incorretos.", htmlContent);
    }

    [Fact]
    public async Task Login_DeveBarrearRequisicao_QuandoCamposObrigatoriosForemVazios()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/Auth/Login");
        
        foreach (var cookie in _securityCookies) 
            request.Headers.TryAddWithoutValidation("Cookie", cookie);

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "__RequestVerificationToken", _antiForgeryToken },
            { "Email", "" },
            { "Password", "" }
        });

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var htmlContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("action=\"/Auth/Login\"", htmlContent);
        Assert.Contains("type=\"password\"", htmlContent);
    }

    [Fact]
    public async Task RedefinirSenha_DeveAlterarSenhaNoBanco_QuandoFluxoForConcluidoComSucesso()
    {
        // Arrange
        const string email = "camila@admin.com";
        const string senhaAntiga = "SenhaTemporaria123!";
        const string senhaNova = "SenhaNova123!";

        using (var scope = _factory.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                await userManager.DeleteAsync(existingUser);
            }

            var user = new UserModel
            {
                UserName = email,
                Email = email,
                Name = "Camila Admin",
                Profile = ProfileEnum.Admin
            };
            var createResult = await userManager.CreateAsync(user, senhaAntiga);
            createResult.Succeeded.Should().BeTrue();
        }

        // Act 1: ForgotPassword
        var forgotRequest = new HttpRequestMessage(HttpMethod.Post, "/Auth/ForgotPassword");
        foreach (var cookie in _securityCookies)
        {
            forgotRequest.Headers.TryAddWithoutValidation("Cookie", cookie);
        }
        forgotRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "__RequestVerificationToken", _antiForgeryToken },
            { "Email", email }
        });

        var forgotResponse = await _client.SendAsync(forgotRequest);

        // Assert 1
        forgotResponse.StatusCode.Should().Be(HttpStatusCode.Redirect);
        _callbackUrl.Should().NotBeNullOrWhiteSpace();

        // Extração do token
        var callbackUri = new Uri(_callbackUrl);
        var query = HttpUtility.ParseQueryString(callbackUri.Query);
        var callbackEmail = query["email"];
        var callbackToken = query["token"];

        callbackEmail.Should().Be(email);
        callbackToken.Should().NotBeNullOrWhiteSpace();

        // Act 2: ResetPassword
        var resetRequest = new HttpRequestMessage(HttpMethod.Post, "/Auth/ResetPassword");
        foreach (var cookie in _securityCookies)
        {
            resetRequest.Headers.TryAddWithoutValidation("Cookie", cookie);
        }
        resetRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "__RequestVerificationToken", _antiForgeryToken },
            { "Email", callbackEmail! },
            { "Token", callbackToken! },
            { "NewPassword", senhaNova },
            { "ConfirmPassword", senhaNova }
        });

        var resetResponse = await _client.SendAsync(resetRequest);

        // Assert 2
        resetResponse.StatusCode.Should().Be(HttpStatusCode.Redirect);
        resetResponse.Headers.Location?.OriginalString.Should().Be("/Auth/Login");

        // Assert Final
        using (var scope = _factory.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            var user = await userManager.FindByEmailAsync(email);
            user.Should().NotBeNull();

            var senhaAntigaValida = await userManager.CheckPasswordAsync(user!, senhaAntiga);
            var senhaNovaValida = await userManager.CheckPasswordAsync(user!, senhaNova);

            senhaAntigaValida.Should().BeFalse();
            senhaNovaValida.Should().BeTrue();
        }
    }
}