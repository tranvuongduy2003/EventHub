using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.Function;

public class UpdateFunctionDto : CreateFunctionDto
{
    [SwaggerSchema("Id of the function")]
    public string Id { get; set; }
}
