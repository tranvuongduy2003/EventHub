using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EventHub.Shared.DTOs.Category;

public class CreateCategoryDto
{
    [DefaultValue("Music")]
    [Description("Name of the category")]
    public string Name { get; set; }
    
    [Description("Icon image of the category")]
    public IFormFile IconImage { get; set; }

    [DefaultValue("blue")]
    [Description("Background color of the category (format: HEX, RGB, RGBA, text)")]
    public string Color { get; set; }
}