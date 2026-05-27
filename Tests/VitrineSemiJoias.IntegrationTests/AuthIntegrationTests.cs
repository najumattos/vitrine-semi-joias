using System.Net;
using System.Text.RegularExpressions;

namespace VitrineSemiJoias.IntegrationTests;

// 💡 Herdando IAsyncLifetime para ativar o ciclo de vida assíncrono do xUnit
public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private string _antiForgeryToken = string.Empty;
    private IEnumerable<string> _securityCookies = Enumerable.Empty<string>();

    public AuthIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
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
            { "Email", "camila@admin.com" },
            { "Password", "123456" }
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
            { "Email", "camila@admin.com" },
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
}