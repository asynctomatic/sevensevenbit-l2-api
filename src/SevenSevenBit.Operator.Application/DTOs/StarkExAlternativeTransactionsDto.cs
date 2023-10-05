namespace SevenSevenBit.Operator.Application.DTOs;

using System.Text.Json.Serialization;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public class StarkExAlternativeTransactionsDto
{
    [JsonPropertyName("alt_txs")]
    public IEnumerable<TransactionModel> AltTxs { get; set; }
}