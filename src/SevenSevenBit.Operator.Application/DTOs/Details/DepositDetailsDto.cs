namespace SevenSevenBit.Operator.Application.DTOs.Details;

using Swashbuckle.AspNetCore.Annotations;

public record DepositDetailsDto
{
    [SwaggerSchema(
        Title = "Metadata",
        Description = "Metadata to reconstruct the deposit transaction.",
        Format = "json")]
    public DepositDetailsMetadata Metadata { get; init; }

    [SwaggerSchema(
        Title = "Signable",
        Description = "The signable deposit transaction.",
        Format = "string")]
    public string Signable { get; init; }
}

public record DepositDetailsMetadata
{
    [SwaggerSchema(
        Title = "Nonce",
        Description = "A sequentially incrementing counter which indicates the transaction number from the account.",
        Format = "hex")]
    public string Nonce { get; init; }

    [SwaggerSchema(
        Title = "GasLimit",
        Description = "The maximum amount of gas units that can be consumed by the transaction.",
        Format = "hex")]
    public string GasLimit { get; init; }

    [SwaggerSchema(
        Title = "MaxPriorityFeePerGas",
        Description = "The maximum price of the consumed gas to be included as a tip to the validator.",
        Format = "hex")]
    public string MaxPriorityFeePerGas { get; init; }

    [SwaggerSchema(
        Title = "MaxFeePerGas",
        Description = " the maximum fee per unit of gas willing to be paid for the transaction.",
        Format = "hex")]
    public string MaxFeePerGas { get; init; }

    [SwaggerSchema(
        Title = "To",
        Description = "The receiving address (EOA or contract account).",
        Format = "hex")]
    public string To { get; init; }

    [SwaggerSchema(
        Title = "Value",
        Description = "Amount of ETH to transfer from sender to recipient.",
        Format = "hex")]
    public string Value { get; init; }

    [SwaggerSchema(
        Title = "Data",
        Description = "Optional field to include arbitrary data.",
        Format = "hex")]
    public string Data { get; init; }
}