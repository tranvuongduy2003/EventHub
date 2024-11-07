using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Category;

public class UpdateCategoryDto : CreateCategoryDto
{
    [DefaultValue("a1b2c3x4y5z6a1b2c3x4y5z6")]
    [SwaggerSchema("Id of the category")]
    public Guid Id { get; set; }
}