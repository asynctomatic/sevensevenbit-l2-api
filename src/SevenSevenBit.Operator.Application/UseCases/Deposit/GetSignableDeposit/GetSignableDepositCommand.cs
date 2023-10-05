namespace SevenSevenBit.Operator.Application.UseCases.Deposit.GetSignableDeposit;

using System.Numerics;
using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Domain.Entities;

public record GetSignableDepositCommand(
    User User,
    Asset Asset,
    Vault Vault,
    BigInteger Amount) : IRequest<Result<SignableDeposit>>;