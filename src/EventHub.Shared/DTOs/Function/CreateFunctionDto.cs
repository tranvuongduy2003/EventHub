using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Function;

public class CreateFunctionDto
{
    [DefaultValue("PAYMENT")]
    [SwaggerSchema("Name of the function")]
    public string Name { get; set; }

    [DefaultValue("/general/payment")]
    [SwaggerSchema("URL of the function")]
    public string Url { get; set; }

    [DefaultValue(1)]
    [SwaggerSchema("Sorting order of the function")]
    public int SortOrder { get; set; }

    [DefaultValue("GENERAL")]
    [SwaggerSchema("Parent function id of the function")]
    public string? ParentId { get; set; }
}