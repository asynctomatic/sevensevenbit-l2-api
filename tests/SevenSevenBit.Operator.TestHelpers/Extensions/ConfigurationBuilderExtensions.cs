namespace SevenSevenBit.Operator.TestHelpers.Extensions;

using Microsoft.Extensions.Configuration;

/// <summary>
/// Extension method for adding appsettings to an <see cref="IConfigurationBuilder"/>.
/// </summary>
public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds appsettings to the configuration builder based on the current environment.
    /// </summary>
    /// <param name="configurationBuilder">The configuration builder.</param>
    public static void AddAppSettings(this IConfigurationBuilder configurationBuilder)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var appSettingsFileName = env == "Production" ? "appsettings.json" : $"appsettings.{env}.json";
        configurationBuilder.AddJsonFile(appSettingsFileName, false, true);
    }
}