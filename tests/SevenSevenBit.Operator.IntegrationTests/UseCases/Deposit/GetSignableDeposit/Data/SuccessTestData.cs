namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Deposit.GetSignableDeposit.Data;

using System.Collections;
using System.Numerics;
using Application.DTOs.Details;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api;

public class SuccessTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Deposit ETH with amount equal to 1 (quantized).
        yield return new object[]
        {
            new SignableDepositModel
            {
                UserId = Users.Alice.Id,
                AssetId = Assets.Eth.Id,
                Amount = Assets.Eth.Quantum.Value,
            },
            new DepositDetailsDto
            {
                Metadata = new DepositDetailsMetadata
                {
                    
                },
                Signable = "placeholder",
            },
        };

        // Deposit ETH with amount larger than 1 (quantized).
        yield return new object[]
        {
            new SignableDepositModel
            {
                UserId = Users.Alice.Id,
                AssetId = Assets.Eth.Id,
                Amount = BigInteger.One,
            },
        };

        // Deposit Erc721 (ID 0x1) with amount equal to 1 (quantized).
        yield return new object[]
        {
            new SignableDepositModel
            {
                UserId = Users.Alice.Id,
                AssetId = Assets.Erc721.Id,
                TokenId = "0x1",
                Amount = BigInteger.Parse("10000000"),
            },
        };

        // Deposit Erc721 (ID 0x2) with amount equal to 1 (quantized).
        yield return new object[]
        {
            new SignableDepositModel
            {
                UserId = Users.Alice.Id,
                AssetId = Assets.Erc721.Id,
                TokenId = "0x2",
                Amount = BigInteger.One,
            },
        };

        // TODO deposit ERC1155.
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}