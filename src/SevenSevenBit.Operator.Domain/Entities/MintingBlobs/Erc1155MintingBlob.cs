namespace SevenSevenBit.Operator.Domain.Entities.MintingBlobs;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SevenSevenBit.Operator.Domain.Common;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

[Table("erc1155_minting_blobs")]
public class Erc1155MintingBlob : BaseMintingBlob
{
}