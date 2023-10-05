namespace SevenSevenBit.Operator.IntegrationTests.Helpers;

using WireMock.Logging;
using WireMock.Server;
using WireMock.Settings;

public static class WiremockHelper
{
    public static WireMockServer StartWiremockServer(int port, bool useLogger = false)
    {
        var settings = CreateWireMockServerSettings(port, useLogger);

        return WireMockServer.Start(settings);
    }

    private static WireMockServerSettings CreateWireMockServerSettings(
        int port,
        bool useLogger = true)
    {
        return new WireMockServerSettings
        {
            Port = port,
            Logger = (useLogger ? new WireMockConsoleLogger() : null)!,
        };
    }
}