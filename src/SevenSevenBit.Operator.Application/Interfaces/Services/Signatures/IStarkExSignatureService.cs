namespace SevenSevenBit.Operator.Application.Interfaces.Services.Signatures;

using SevenSevenBit.Operator.Domain.ValueObjects;

public interface IStarkExSignatureService
{
    bool ValidateStarkExSignature(
        string messageHash,
        StarkKey starkKey,
        StarkSignature signature);
}