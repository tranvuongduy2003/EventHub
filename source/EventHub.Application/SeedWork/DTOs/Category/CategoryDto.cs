using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Category;

public class CategoryDto
{
    [SwaggerSchema("Id of the category")]
    public Guid Id { get; set; }

    [SwaggerSchema("Name of the category")]
    public string Name { get; set; }

    [SwaggerSchema("Icon image URL of the category")]
    public string IconImageUrl { get; set; }

    [SwaggerSchema("Background color of the category")]
    public string Color { get; set; }

    [SwaggerSchema("The datetime that the category was created")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema("The last datetime that the category was updated")]
    public DateTime? UpdatedAt { get; set; }
}
