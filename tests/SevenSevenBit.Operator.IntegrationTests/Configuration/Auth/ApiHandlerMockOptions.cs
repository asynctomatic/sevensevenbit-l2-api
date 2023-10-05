namespace SevenSevenBit.Operator.IntegrationTests.Configuration.Auth;

using Microsoft.AspNetCore.Authentication;

public class ApiHandlerMockOptions : AuthenticationSchemeOptions
{
    public string DefaultClientId { get; set; }
}