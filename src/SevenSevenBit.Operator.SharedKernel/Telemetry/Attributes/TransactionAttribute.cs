namespace SevenSevenBit.Operator.SharedKernel.Telemetry.Attributes;

using NRTransactionAttribute = NewRelic.Api.Agent.TransactionAttribute;

[AttributeUsage(AttributeTargets.Method)]
public class TransactionAttribute : NRTransactionAttribute
{
}