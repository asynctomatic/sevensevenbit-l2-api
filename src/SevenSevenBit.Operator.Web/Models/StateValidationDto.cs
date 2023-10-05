namespace SevenSevenBit.Operator.Web.Models;

using Microsoft.AspNetCore.Mvc;

public class StateValidationDto
{
    public StateValidationDto()
    {
        IsValid = true;
    }

    public StateValidationDto(ObjectResult result)
    {
        Result = result;
    }

    public bool IsValid { get; set; }

    public ObjectResult Result { get; set; }
}