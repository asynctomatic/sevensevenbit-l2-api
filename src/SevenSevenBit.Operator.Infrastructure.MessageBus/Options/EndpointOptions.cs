namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Options;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents configuration options for message bus endpoints.
/// </summary>
[ExcludeFromCodeCoverage]
public class EndpointOptions
{
    public string AllocateTransactionId { get; set; }

    public string FreeTransactionId { get; set; }

    public string SubmitTransaction { get; set; }
}