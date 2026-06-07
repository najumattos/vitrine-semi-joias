using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace VitrineSemiJoias.IntegrationTests;

internal sealed class AlwaysValidAntiforgery : IAntiforgery
{
    public AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext) =>
        new(string.Empty, string.Empty, string.Empty, string.Empty);

    public AntiforgeryTokenSet GetTokens(HttpContext httpContext) =>
        new(string.Empty, string.Empty, string.Empty, string.Empty);

    public Task<bool> IsRequestValidAsync(HttpContext httpContext) =>
        Task.FromResult(true);

    public Task ValidateRequestAsync(HttpContext httpContext) =>
        Task.CompletedTask;

    public void SetCookieTokenAndHeader(HttpContext httpContext)
    {
    }
}
