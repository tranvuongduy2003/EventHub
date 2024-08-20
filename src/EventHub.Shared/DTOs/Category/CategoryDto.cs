using System.ComponentModel;

namespace EventHub.Shared.DTOs.Category;

public class CategoryDto
{
    [Description("Id of the category")]
    public string Id { get; set; }

    [Description("Name of the category")]
    public string Name { get; set; }

    [Description("Icon image URL of the category")]
    public string IconImage { get; set; }

    [Description("Background color of the category")]
    public string Color { get; set; }

    [Description("The datetime that the category was created")]
    public DateTime CreatedAt { get; set; }

    [Description("The last datetime that the category was updated")]
    public DateTime? UpdatedAt { get; set; }
}