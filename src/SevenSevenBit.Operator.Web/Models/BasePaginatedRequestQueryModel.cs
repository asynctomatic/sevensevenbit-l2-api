namespace SevenSevenBit.Operator.Web.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class BasePaginatedRequestQueryModel
{
    [Range(0, int.MaxValue)]
    [FromQuery(Name = "page_number")]
    [SwaggerSchema(
        Title = "Page Number",
        Description = "The page number to retrieve.",
        Format = "int32")]
    public int? PageNumber { get; set; }

    [Range(0, 100)]
    [FromQuery(Name = "page_size")]
    [SwaggerSchema(
        Title = "Page Size",
        Description = "The number of items to retrieve per page.",
        Format = "int32")]
    public int? PageSize { get; set; }

    [FromQuery(Name = "sort_by")]
    [SwaggerSchema(
        Title = "Sort By",
        Description = "The field to sort the results by.")]
    public string SortBy { get; set; }
}