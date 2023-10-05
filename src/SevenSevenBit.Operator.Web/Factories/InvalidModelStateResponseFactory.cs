namespace SevenSevenBit.Operator.Web.Factories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SevenSevenBit.Operator.Domain.Enums;

public static class InvalidModelStateResponseFactory
{
    public static IActionResult SerializeInvalidModelStateResponse(ActionContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Controller>>();
        var errors = context.ModelState.GetModelStateErrors(logger);

        logger.LogWarning("Model State validation failed ({Errors})", errors);

        ValidationProblemDetails problemDetails = new()
        {
            Detail = "Please refer to the errors property for additional details",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.HttpContext.Request.Path,
            Type = ((int)ErrorCodes.ModelStateInvalid).ToString(),
        };

        foreach (var (key, value) in errors)
        {
            problemDetails.Errors[key] = problemDetails.Errors.TryGetValue(key, out var existingErrors)
                ? existingErrors.Append(value.ToString()).ToArray()
                : new[] { value.ToString() };
        }

        return new BadRequestObjectResult(problemDetails);
    }

    private static IEnumerable<(string Key, int Value)> GetModelStateErrors(
        this ModelStateDictionary modelState,
        ILogger logger)
    {
        return modelState.Keys.SelectMany(
            key => modelState[key].Errors,
            (key, error) =>
            {
                if (!Enum.TryParse<ErrorCodes>(error.ErrorMessage, out var errorCode))
                {
                    logger.LogWarning("Failed to parse Enum ErrorCode {ErrorCode}", error.ErrorMessage);
                    errorCode = ErrorCodes.ModelStateInvalid;
                }

                return (key, (int)errorCode);
            });
    }
}