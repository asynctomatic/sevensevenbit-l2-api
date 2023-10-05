namespace SevenSevenBit.Operator.Application.Comparers;

using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class TransferModelComparer : IEqualityComparer<TransferModel>
{
    public bool Equals(TransferModel x, TransferModel y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (ReferenceEquals(x, null))
        {
            return false;
        }

        if (ReferenceEquals(y, null))
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.Amount.Equals(y.Amount) &&
               x.ExpirationTimestamp.Equals(y.ExpirationTimestamp) &&
               Equals(x.FeeInfoExchange, y.FeeInfoExchange) &&
               Equals(x.FeeInfo, y.FeeInfo) &&
               x.Nonce.Equals(y.Nonce) &&
               string.Equals(x.ReceiverPublicKey, y.ReceiverPublicKey) &&
               x.ReceiverVaultId.Equals(y.ReceiverVaultId) &&
               string.Equals(x.SenderPublicKey, y.SenderPublicKey) &&
               x.SenderVaultId.Equals(y.SenderVaultId) &&
               Equals(x.Signature, y.Signature) &&
               string.Equals(x.Token, y.Token);
    }

    public int GetHashCode(TransferModel obj)
    {
        var hashCode = default(HashCode);
        hashCode.Add(obj.Amount);
        hashCode.Add(obj.ExpirationTimestamp);
        hashCode.Add(obj.FeeInfoExchange);
        hashCode.Add(obj.FeeInfo);
        hashCode.Add(obj.Nonce);
        hashCode.Add(obj.ReceiverPublicKey);
        hashCode.Add(obj.ReceiverVaultId);
        hashCode.Add(obj.SenderPublicKey);
        hashCode.Add(obj.SenderVaultId);
        hashCode.Add(obj.Signature);
        hashCode.Add(obj.Token);
        hashCode.Add(obj.Type);

        return hashCode.ToHashCode();
    }
}