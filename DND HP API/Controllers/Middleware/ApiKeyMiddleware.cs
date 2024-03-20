using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DND_HP_API.Controllers.Middleware;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "ClientKey";
    public const string HeaderName = "x-api-key";
    public string ApiKey { get; set; }
}

[Obsolete("Obsolete")] //because ISystemClock is obsolete.
public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<ApiKeyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock)
    : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder, clock)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderName, out var apiKey) || apiKey.Count != 1)
        {
            Logger.LogWarning("An API request was received without the x-api-key header");
            return AuthenticateResult.Fail("Invalid parameters");
        }

        if (apiKey[0] != Options.ApiKey)
        {
            Logger.LogWarning("An API request was received with an invalid x-api-key header");
            return AuthenticateResult.Fail("Invalid parameters");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Role, "GameMaster")
        };
        var identity = new ClaimsIdentity(claims, ApiKeyAuthenticationOptions.DefaultScheme);
        var identities = new List<ClaimsIdentity> { identity };
        var principal = new ClaimsPrincipal(identities);

        var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationOptions.DefaultScheme);
        return AuthenticateResult.Success(ticket);
    }
}