namespace SevenSevenBit.Operator.Application.DTOs.Internal;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public class Fee
{
    public Asset Asset { get; init; }

    public BigInteger Amount { get; init; } // TODO check for errors when Amount < Quantum - To Resolve in Fee PR

    public BigInteger QuantizedAmount => Amount.ToQuantized(Asset.Quantum);
}