namespace SevenSevenBit.Operator.Web.Models.Api;

using System.Text.Json.Serialization;
using StarkEx.Client.SDK.Enums.Spot;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class StarkExRevertTransactionModel
{
    [JsonPropertyName("tx_id")]
    public long TxId { get; set; }

    [JsonPropertyName("reason_code")]
    public SpotApiCodes ReasonCode { get; set; }

    [JsonPropertyName("reason_msg")]
    public string ReasonMessage { get; set; }

    [JsonPropertyName("tx")]
    public TransactionModel Transaction { get; set; }
}