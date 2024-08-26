using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Function;

public class UpdateFunctionDto : CreateFunctionDto
{
    [DefaultValue("a1b2c3x4y5z6a1b2c3x4y5z6")]
    [SwaggerSchema("Id of the function")]
    public string Id { get; set; }
}