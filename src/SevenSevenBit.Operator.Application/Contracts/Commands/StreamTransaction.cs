namespace SevenSevenBit.Operator.Application.Contracts.Commands;

public record StreamTransaction
{
    public Guid TransactionStreamId { get; init; }

    public Guid StarkExInstanceId { get; init; }

    public string StarkExInstanceBaseAddress { get; init; }

    public string StarkExInstanceApiVersion { get; init; }

    public string Transaction { get; init; }
}