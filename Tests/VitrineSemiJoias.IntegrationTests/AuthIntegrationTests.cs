using System.Net;
using System.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using VitrineSemiJoias.Data;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.IntegrationTests;

public partial class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IEmailSenderService _emailSender;
    
    private const string SeedEmail = "camila@admin.com";
    private const string SeedPassword = "SenhaValida123!";
    private string _callbackUrl = string.Empty;

    public AuthIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _emailSender = factory.EmailSenderMock;

        _emailSender
            .SendPasswordResetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _emailSender
            .When(call => call.SendPasswordResetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>()))
            .Do(call => _callbackUrl = call.ArgAt<string>(1));

        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    public async Task InitializeAsync()
    {
        using var scope = _factory.Server.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureCreatedAsync();

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
        var createResult = await userManager.CreateAsync(user, SeedPassword);
        createResult.Succeeded.Should().BeTrue(
            string.Join("; ", createResult.Errors.Select(e => e.Description)));
        (await userManager.CheckPasswordAsync(user, SeedPassword)).Should().BeTrue();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Login_DeveAutenticarERedirecionar_QuandoCredenciaisForemValidas()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/Auth/Login")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "__RequestVerificationToken", "" },
                { "Email", SeedEmail },
                { "Password", SeedPassword }
            })
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location?.OriginalString.Should().Contain("/Products");
    }

    [Fact]
    public async Task Login_DeveRetornarMesmaViewComErro_QuandoEmailNaoExistir()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/Auth/Login")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "__RequestVerificationToken", "" },
                { "Email", "email-fantasma@naoexiste.com" },
                { "Password", "eu-preciso-de-um-emprego" }
            })
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK); 
        var htmlContent = await response.Content.ReadAsStringAsync();
        htmlContent.Should().Contain("Senha ou email incorretos."); 
    }

    [Fact]
    public async Task Login_DeveRetornarMesmaViewComErro_QuandoSenhaForIncorreta()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/Auth/Login")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "__RequestVerificationToken", "" },
                { "Email", SeedEmail },
                { "Password", "SenhaErrada123!" }
            })
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var htmlContent = await response.Content.ReadAsStringAsync();
        htmlContent.Should().Contain("Senha ou email incorretos.");
    }

    [Fact]
    public async Task Login_DeveBarrearRequisicao_QuandoCamposObrigatoriosForemVazios()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/Auth/Login")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "__RequestVerificationToken", "" },
                { "Email", "" },
                { "Password", "" }
            })
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var htmlContent = await response.Content.ReadAsStringAsync();
        htmlContent.Should().Contain("action=\"/Auth/Login\"");
        htmlContent.Should().Contain("type=\"password\"");
    }

    [Fact]
    public async Task RedefinirSenha_DeveAlterarSenhaNoBanco_QuandoFluxoForConcluidoComSucesso()
    {
        // Arrange
        const string emailMutavel = "camila.mutavel@admin.com";
        const string senhaAntiga = "SenhaTemporaria123!";
        const string senhaNova = "SenhaNova123!";

        using (var scope = _factory.Server.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            
            var user = new UserModel
            {
                UserName = emailMutavel,
                Email = emailMutavel,
                Name = "Camila Mutável",
                Profile = ProfileEnum.Admin
            };
            var createResult = await userManager.CreateAsync(user, senhaAntiga);
            createResult.Succeeded.Should().BeTrue(
                string.Join("; ", createResult.Errors.Select(e => e.Description)));
        }

        // Act 1: ForgotPassword
        var forgotRequest = new HttpRequestMessage(HttpMethod.Post, "/Auth/ForgotPassword")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "__RequestVerificationToken", "" },
                { "Email", emailMutavel }
            })
        };

        var forgotResponse = await _client.SendAsync(forgotRequest);

        // Assert 1
        forgotResponse.StatusCode.Should().Be(HttpStatusCode.Redirect);
        _callbackUrl.Should().NotBeNullOrWhiteSpace();

        var callbackUri = new Uri(_callbackUrl);
        var query = HttpUtility.ParseQueryString(callbackUri.Query);
        var callbackEmail = query["email"];
        var callbackToken = query["token"];

        callbackEmail.Should().Be(emailMutavel);
        callbackToken.Should().NotBeNullOrWhiteSpace();

        // Act 2: ResetPassword
        var resetRequest = new HttpRequestMessage(HttpMethod.Post, "/Auth/ResetPassword")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "__RequestVerificationToken", "" },
                { "Email", callbackEmail! },
                { "Token", callbackToken! },
                { "NewPassword", senhaNova },
                { "ConfirmPassword", senhaNova }
            })
        };

        var resetResponse = await _client.SendAsync(resetRequest);

        // Assert 2
        resetResponse.StatusCode.Should().Be(HttpStatusCode.Redirect);
        resetResponse.Headers.Location?.OriginalString.Should().Be("/Auth/Login");

        // Assert Final
        using (var scope = _factory.Server.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            var user = await userManager.FindByEmailAsync(emailMutavel);
            user.Should().NotBeNull();

            var senhaAntigaValida = await userManager.CheckPasswordAsync(user!, senhaAntiga);
            var senhaNovaValida = await userManager.CheckPasswordAsync(user!, senhaNova);

            senhaAntigaValida.Should().BeFalse();
            senhaNovaValida.Should().BeTrue();
        }
    }
}