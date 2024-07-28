using Microsoft.AspNetCore.Http;

namespace EventHub.Domain.DTOs.Category;

public class CreateCategoryDto
{
    public string Name { get; set; }

    // Image name
    public IFormFile IconImage { get; set; }

    public string Color { get; set; }
}