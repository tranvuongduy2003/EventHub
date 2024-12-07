using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Function;

public class UpdateFunctionDto : CreateFunctionDto
{
    [SwaggerSchema("Id of the function")]
    public string Id { get; set; }
}
