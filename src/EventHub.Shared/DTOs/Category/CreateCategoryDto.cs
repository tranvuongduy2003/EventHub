using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Category;

public class CreateCategoryDto
{
    [DefaultValue("Music")]
    [SwaggerSchema("Name of the category")]
    public string Name { get; set; }
    
    [SwaggerSchema("Icon image of the category")]
    public IFormFile IconImage { get; set; }

    [DefaultValue("blue")]
    [SwaggerSchema("Background color of the category (format: HEX, RGB, RGBA, text)")]
    public string Color { get; set; }
}