namespace SevenSevenBit.Operator.Application.Exceptions;

using Microsoft.AspNetCore.Mvc;

public abstract class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }

    // public ProblemDetails ToProblemDetails()
    // {
    //     return new ProblemDetails
    //     {
    //         Title = "Same base and quote asset.",
    //         Detail = $"The base asset ID and quote asset ID must be different.",
    //         Status = StatusCodes.Status400BadRequest,
    //         Type = ((int)ErrorCodes.SameBaseAndQuoteAssets).ToString(),
    //         Instance = HttpContext.Request.Path,
    //     };
    // }
}

public class AssetNotFoundException : NotFoundException
{
    public AssetNotFoundException(Guid assetId)
        : base($"Asset with ID {assetId} not found.")
    {
    }
}

public class MarketplaceNotFoundException : NotFoundException
{
    public MarketplaceNotFoundException(Guid marketplaceId)
        : base($"Marketplace with ID {marketplaceId} not found.")
    {
    }
}

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid userId)
        : base($"User with ID {userId} not found.")
    {
    }
}

public class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(Guid orderId)
        : base($"Order with ID {orderId} not found.")
    {
    }
}