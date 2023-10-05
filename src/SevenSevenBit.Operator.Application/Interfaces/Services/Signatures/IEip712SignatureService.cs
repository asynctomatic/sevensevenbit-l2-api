namespace SevenSevenBit.Operator.Application.Interfaces.Services.Signatures;

using Nethereum.ABI.EIP712;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;

public interface IEip712SignatureService
{
    bool ValidateEip712Signature(
        SignableMethods signableMethod,
        MemberValue[] message,
        string signature,
        BlockchainAddress address);

    TypedData<Domain> GetTypedData(
        SignableMethods signableMethod,
        MemberValue[] message);
}