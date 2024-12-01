using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Function;

public class UpdateFunctionDto : CreateFunctionDto
{
    [SwaggerSchema("Id of the function")]
    public string Id { get; set; }
}
