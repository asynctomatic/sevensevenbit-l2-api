namespace SevenSevenBit.Operator.Infrastructure.Identity.Extensions;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

[ExcludeFromCodeCoverage]
public static class AuthorizationExtensions
{
    private static readonly IEnumerable<string> ScopeClaimTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "http://schemas.microsoft.com/identity/claims/scope",
        "scope",
    };

    // Microsoft current implementation still can't handle the newest OAuth 2.0 spec where the scope claim
    // is a single string containing a space-separated list of scopes associated with the token.
    // See https://datatracker.ietf.org/doc/html/rfc8693#section-4.2 for more details
    public static void RequireScope(this AuthorizationPolicyBuilder builder, params string[] scopes)
    {
        builder.RequireAssertion(context =>
            context.User
                .Claims
                .Where(c => ScopeClaimTypes.Contains(c.Type))
                .SelectMany(c => c.Value.Split(' '))
                .Any(s => scopes.Contains(s, StringComparer.Ordinal)));
    }
}