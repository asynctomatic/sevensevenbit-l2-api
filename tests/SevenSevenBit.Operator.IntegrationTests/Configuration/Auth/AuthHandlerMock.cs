namespace SevenSevenBit.Operator.IntegrationTests.Configuration.Auth;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Domain.Constants.Auth;

public class AuthHandlerMock : AuthenticationHandler<ApiHandlerMockOptions>
{
    public const string AuthScheme = "TestScheme";
    public const string TestingTenantAuthHeader = "TestingTenant";
    public const string AdminAuthHeader = "Admin";
    public const string StarkExAuthHeader = "StarkEx";

    private const string ApiScopes = "read:users write:users read:assets write:assets mint:assets read:vaults write:vaults read:transfers write:transfers read:transactions write:settlements write:marketplaces read:marketplaces write:orders read:orders";
    private const string AdminScopes = "";  // TODO: REMOVE THIS.
    private const string StarkExScopes = "alt_txs";
    private const string TestingClientId = "QyGf33tuI4GMJquSOpOZauWsJ6xxB7sL";
    private const string AdminClientId = "szfgRzFAdQZVzCp7IZIZswGbprHHDbpx";
    private const string StarkExClientId = "pClUPlUobie8SySSMY7T3zknvj2jzG7r";
    private const string AdminName = "Admin";
    private const string StarkExName = "StarkEx";

    private readonly string defaultClientId;

    public AuthHandlerMock(
        IOptionsMonitor<ApiHandlerMockOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        defaultClientId = options.CurrentValue.DefaultClientId;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            authHeader = TestingTenantAuthHeader;
        }

        var authToken = authHeader.ToString();

        IEnumerable<Claim> claims;
        if (authToken.StartsWith("Bearer "))
        {
            var jwt = authToken.Split(' ')[1];
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(jwt);
            claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, jwtSecurityToken.Subject),
                new(ClaimTypes.Email, jwtSecurityToken.Claims.First(x => x.Type == "email").Value),
                new("scope", jwtSecurityToken.Claims.First(x => x.Type == "scope").Value),
            };
        }
        else
        {
            claims = new List<Claim>
            {
                new("azp", GetClientId(authHeader)),
                new("scope", GetScopes(authHeader)),
            };
        }

        var identity = new ClaimsIdentity(claims, TestingTenantAuthHeader);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, TestingTenantAuthHeader);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }

    private string GetClientId(string authHeader)
    {
        return authHeader switch
        {
            TestingTenantAuthHeader => TestingClientId,
            AdminAuthHeader => AdminClientId,
            StarkExAuthHeader => StarkExClientId,
            _ => defaultClientId,
        };
    }

    private string GetScopes(string authHeader)
    {
        return authHeader switch
        {
            TestingTenantAuthHeader => ApiScopes,
            AdminAuthHeader => AdminScopes,
            StarkExAuthHeader => StarkExScopes,
            _ => defaultClientId,
        };
    }
}