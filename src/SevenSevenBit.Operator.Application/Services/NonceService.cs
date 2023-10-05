namespace SevenSevenBit.Operator.Application.Services;

using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using SevenSevenBit.Operator.Application.Interfaces.Services;

public class NonceService : INonceService
{
    private readonly ILogger<NonceService> logger;

    public NonceService(ILogger<NonceService> logger)
    {
        this.logger = logger;
    }

    public int GetRandomNonce()
    {
        var byteArray = new byte[4];
        RandomNumberGenerator.Fill(byteArray);

        // StarkEx nonce is capped to in32 max value. Docs state uint but api fails with values > int32.maxValue
        var nonce = Math.Abs(BitConverter.ToInt32(byteArray, 0));

        logger.LogInformation("Generated nonce: {Nonce}", nonce);

        return nonce;
    }
}