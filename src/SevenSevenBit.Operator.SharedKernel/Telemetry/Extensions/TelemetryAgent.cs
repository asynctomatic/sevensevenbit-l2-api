namespace SevenSevenBit.Operator.SharedKernel.Telemetry.Extensions;

using System.Diagnostics.CodeAnalysis;
using NewRelic.Api.Agent;

// TODO Make sure runtimes have newrelic serilog
[ExcludeFromCodeCoverage]
public static class TelemetryAgent
{
    public static void AddCustomAttribute(string key, string value)
    {
        var agent = NewRelic.GetAgent();
        var transaction = agent.CurrentTransaction;
        transaction.AddCustomAttribute(key, value);
    }
}