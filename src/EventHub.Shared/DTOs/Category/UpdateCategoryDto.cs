using System.ComponentModel;

namespace EventHub.Shared.DTOs.Category;

public class UpdateCategoryDto : CreateCategoryDto
{
    [DefaultValue("a1b2c3x4y5z6a1b2c3x4y5z6")]
    [Description("Id of the category")]
    public Guid Id { get; set; }
}