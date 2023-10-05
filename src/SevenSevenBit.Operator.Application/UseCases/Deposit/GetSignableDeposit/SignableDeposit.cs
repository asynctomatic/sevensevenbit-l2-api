namespace SevenSevenBit.Operator.Application.UseCases.Deposit.GetSignableDeposit;

public record SignableDeposit(
    string Nonce,
    string GasLimit,
    string MaxPriorityFeePerGas,
    string MaxFeePerGas,
    string To,
    string Value,
    string Data);