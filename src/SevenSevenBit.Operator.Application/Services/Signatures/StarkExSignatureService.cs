namespace SevenSevenBit.Operator.Application.Services.Signatures;

using Microsoft.Extensions.Logging;
using SevenSevenBit.Operator.Application.Interfaces.Services.Signatures;
using SevenSevenBit.Operator.Domain.ValueObjects;
using StarkEx.Crypto.SDK.Signing;

public class StarkExSignatureService : IStarkExSignatureService
{
    private readonly ILogger<StarkExSignatureService> logger;
    private readonly IStarkExSigner starkExSigner;

    public StarkExSignatureService(
        ILogger<StarkExSignatureService> logger,
        IStarkExSigner starkExSigner)
    {
        this.logger = logger;
        this.starkExSigner = starkExSigner;
    }

    public bool ValidateStarkExSignature(
        string messageHash,
        StarkKey starkKey,
        StarkSignature signature)
    {
        try
        {
            return starkExSigner.VerifySignature(messageHash, starkKey, signature);
        }
        catch (Exception ex)
        {
            logger.LogError(
                    ex,
                    "There was an error validating a Stark signature (R: {R}, S: {S}) with StarkKey {StarkKey}",
                    signature.R,
                    signature.S,
                    starkKey);
        }

        return false;
    }
}