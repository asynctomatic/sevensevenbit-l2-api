namespace SevenSevenBit.Operator.Web.Docs.Filters;

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class TagDescriptionsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags = new List<OpenApiTag>
        {
            new OpenApiTag { Name = "User", Description = "Manage user-related operations." },
            new OpenApiTag { Name = "Asset", Description = "Manage asset-related operations." },
            new OpenApiTag { Name = "Mint", Description = "Manage minting-related operations." },
            new OpenApiTag { Name = "Transfer", Description = "Manage asset transfer operations." },
            new OpenApiTag { Name = "Transaction", Description = "Manage transaction-related operations." },
            new OpenApiTag { Name = "Withdraw", Description = "Manage asset withdrawal operations." },
            new OpenApiTag { Name = "Fee", Description = "Manage fee configuration operations." },
            new OpenApiTag { Name = "Order", Description = "Manage order-related operations." },
            new OpenApiTag { Name = "Settlement", Description = "Manage asset settlement operations." },
            new OpenApiTag { Name = "Marketplace", Description = "Manage marketplace-related operations." },
            new OpenApiTag { Name = "Deposit", Description = "Manage asset deposit operations." },
        };
    }
}