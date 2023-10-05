namespace SevenSevenBit.Operator.IntegrationTests.Configuration;

using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using SevenSevenBit.Operator.IntegrationTests.Configuration.Auth;

// TODO: Remove this class.
public static class WebApplicationFactoryExtensions
{
    private const string Version = "v1";

    public static HttpClient CreateAuthorizedApiClient<T>(
        this WebApplicationFactory<T> factory)
        where T : class
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri($"https://localhost/api/{Version}/");

        // Use Api Auth scheme
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthHandlerMock.TestingTenantAuthHeader);

        return client;
    }

    public static HttpClient CreateAuthorizedAdminClient<T>(
        this WebApplicationFactory<T> factory)
        where T : class
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri($"https://localhost/admin/{Version}/");

        // Use Admin Auth scheme
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthHandlerMock.AdminAuthHeader);

        return client;
    }

    public static HttpClient CreateAuthorizedStarkExClient<T>(
        this WebApplicationFactory<T> factory)
        where T : class
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri($"https://localhost/starkex/{Version}/");

        // Use StarkEx Auth scheme
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthHandlerMock.StarkExAuthHeader);

        return client;
    }

    public static HttpClient CreateUnauthorizedAdminClient<T>(
        this WebApplicationFactory<T> factory)
        where T : class
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri($"https://localhost/admin/{Version}/");

        return client;
    }
}