using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.Category;

public class CreateCategoryDto
{
    [SwaggerSchema("Name of the category")]
    public string Name { get; set; }

    [SwaggerSchema("Icon image of the category")]
    public IFormFile IconImage { get; set; }

    [SwaggerSchema("Background color of the category (format: HEX, RGB, RGBA, text)")]
    public string Color { get; set; }
}
