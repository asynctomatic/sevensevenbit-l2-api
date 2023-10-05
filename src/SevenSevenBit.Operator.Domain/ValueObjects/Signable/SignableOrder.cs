namespace SevenSevenBit.Operator.Domain.ValueObjects.Signable;

using System.Numerics;

public class SignableOrder
{
    public SignableOrder(
        BigInteger baseAssetAmountQuantized,
        BigInteger quoteAssetAmountQuantized,
        int nonce,
        long expirationTimestamp,
        string signable)
    {
        BaseAssetAmountQuantized = baseAssetAmountQuantized;
        QuoteAssetAmountQuantized = quoteAssetAmountQuantized;
        Nonce = nonce;
        ExpirationTimestamp = expirationTimestamp;
        Signable = signable;
    }

    public BigInteger BaseAssetAmountQuantized { get; }

    public BigInteger QuoteAssetAmountQuantized { get; }

    public int Nonce { get; }

    public long ExpirationTimestamp { get; }

    public string Signable { get; }
}