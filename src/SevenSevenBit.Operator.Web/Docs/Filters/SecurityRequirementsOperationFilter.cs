namespace SevenSevenBit.Operator.Web.Docs.Filters;

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Adds "401" and "403" responses for all actions that are decorated with a <see cref="AuthorizeAttribute"/>.
/// </summary>
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Policy names map to scopes
        var requiredPolicies = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Select(attr => attr.Policy)
            .Distinct();

        var requiredScopes = requiredPolicies.SelectMany(policy => policy.GetScopes());
        var enumerableScopes = requiredScopes as string[] ?? requiredScopes.ToArray();
        if (enumerableScopes.Any())
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized." });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden." });

            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    [oAuthScheme] = enumerableScopes.ToList(),
                },
            };
        }
    }
}