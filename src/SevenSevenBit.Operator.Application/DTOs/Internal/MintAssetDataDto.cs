namespace SevenSevenBit.Operator.Application.DTOs.Internal;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;

public class MintAssetDataDto
{
    public User User { get; set; }

    public Asset Asset { get; set; }

    public DataAvailabilityModes DataAvailabilityMode { get; set; }

    public string MintingBlob { get; set; }

    public BigInteger QuantizedAmount { get; set; }
}