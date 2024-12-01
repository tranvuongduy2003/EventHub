using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Category;

public class UpdateCategoryDto : CreateCategoryDto
{
    [SwaggerSchema("Id of the category")]
    public Guid Id { get; set; }
}
