namespace SevenSevenBit.Operator.Infrastructure.Identity.Options;

public class AuthenticationOptions
{
    public const string Authentication = "Authentication";

    public string Audience { get; set; }

    public string Authority { get; set; }
}