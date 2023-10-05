namespace SevenSevenBit.Operator.Domain.ValueObjects;

using System.Text.Json.Serialization;
using Nethereum.Hex.HexConvertors.Extensions;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using StarkEx.Commons.SDK.Models;

public record StarkSignature
{
    [JsonConstructor]
    public StarkSignature(string r, string s)
    {
        if (r is null)
        {
            throw new ArgumentNullException(nameof(r), "R cannot be null.");
        }

        if (!r.IsValidHexString())
        {
            throw new ArgumentException("R must be a valid hex string.");
        }

        if (s is null)
        {
            throw new ArgumentNullException(nameof(s), "S cannot be null.");
        }

        if (!s.IsValidHexString())
        {
            throw new ArgumentException("S must be a valid hex string.");
        }

        R = r.EnsureHexPrefix();
        S = s.EnsureHexPrefix();
    }

    public string R { get; }

    public string S { get; }

    public static implicit operator SignatureModel(StarkSignature starkKey) => new()
    {
        R = starkKey.R.EnsureHexPrefix(),
        S = starkKey.S.EnsureHexPrefix(),
    };

    public static implicit operator StarkSignature(SignatureModel signatureModel) => new(signatureModel.R, signatureModel.S);
}