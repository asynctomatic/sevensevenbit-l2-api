namespace SevenSevenBit.Operator.Application.UseCases.Deposit.GetSignableDeposit;

using System.Globalization;
using System.Numerics;
using LanguageExt.Common;
using MediatR;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using SevenSevenBit.Operator.Application.UseCases.Deposit.GetSignableDeposit.Functions;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using StarkEx.Crypto.SDK.Enums;

public class GetSignableDepositCommandHandler : IRequestHandler<GetSignableDepositCommand, Result<SignableDeposit>>
{
    private readonly IWeb3 web3;

    public GetSignableDepositCommandHandler(IWeb3 web3)
    {
        this.web3 = web3;
    }

    public async Task<Result<SignableDeposit>> Handle(GetSignableDepositCommand command, CancellationToken cancellationToken)
    {
        TransactionInput txInput;
        switch (command.Asset.Type)
        {
            case AssetType.Eth:
                txInput = await GetTransactionInputAsync(new DepositEthFunction
                {
                    StarkKey = BigInteger.Parse(command.User.StarkKey.Value[2..], NumberStyles.AllowHexSpecifier),
                    AssetType = BigInteger.Parse(command.Asset.StarkExType.Value[2..], NumberStyles.AllowHexSpecifier),
                    VaultId = command.Vault.VaultChainId.Value,
                });
                break;

            case AssetType.Erc20:
                txInput = await GetTransactionInputAsync(new DepositErc20Function
                {
                    StarkKey = BigInteger.Parse(command.User.StarkKey.Value[2..], NumberStyles.AllowHexSpecifier),
                    AssetType = BigInteger.Parse(command.Asset.StarkExType.Value[2..], NumberStyles.AllowHexSpecifier),
                    VaultId = command.Vault.VaultChainId.Value,
                    QuantizedAmount = command.Amount.ToQuantized(command.Asset.Quantum.Value),
                });
                break;

            case AssetType.Erc721:
                txInput = await GetTransactionInputAsync(new DepositErc721Function
                {
                    StarkKey = BigInteger.Parse(command.User.StarkKey.Value[2..], NumberStyles.AllowHexSpecifier),
                    AssetType = BigInteger.Parse(command.Asset.StarkExType.Value[2..], NumberStyles.AllowHexSpecifier),
                    VaultId = command.Vault.VaultChainId.Value,
                    TokenId = BigInteger.Parse(command.Vault.TokenId.Value[2..], NumberStyles.AllowHexSpecifier),
                });
                break;

            case AssetType.Erc1155:
                txInput = await GetTransactionInputAsync(new DepositErc1155Function
                {
                    StarkKey = BigInteger.Parse(command.User.StarkKey.Value[2..], NumberStyles.AllowHexSpecifier),
                    AssetType = BigInteger.Parse(command.Asset.StarkExType.Value[2..], NumberStyles.AllowHexSpecifier),
                    VaultId = command.Vault.VaultChainId.Value,
                    TokenId = BigInteger.Parse(command.Vault.TokenId.Value[2..], NumberStyles.AllowHexSpecifier),
                    QuantizedAmount = command.Amount.ToQuantized(command.Asset.Quantum.Value),
                });
                break;

            case AssetType.MintableErc20:
            case AssetType.MintableErc721:
            case AssetType.MintableErc1155:
            default:
                // TODO: Custom exception.
                return new Result<SignableDeposit>(new Exception("Asset type not supported"));
        }

        var signableDeposit = new SignableDeposit(
            Nonce: txInput.Nonce.HexValue,
            GasLimit: txInput.Gas.HexValue,
            MaxPriorityFeePerGas: txInput.MaxPriorityFeePerGas.HexValue,
            MaxFeePerGas: txInput.MaxFeePerGas.HexValue,
            To: txInput.To,
            Value: txInput.Value.HexValue,
            Data: txInput.Data);

        return new Result<SignableDeposit>(signableDeposit);
    }

    private async Task<TransactionInput> GetTransactionInputAsync<T>(T functionCall)
        where T : FunctionMessage, new()
    {
        // TODO: handle exception on CreateTransactionInputEstimatingGasAsync.
        var txHandler = web3.Eth.GetContractTransactionHandler<T>();
        // TODO: get contract address from config options.
        return await txHandler.CreateTransactionInputEstimatingGasAsync(
            "0x5fbdb2315678afecb367f032d93f642f64180aa3", functionCall);
    }
}