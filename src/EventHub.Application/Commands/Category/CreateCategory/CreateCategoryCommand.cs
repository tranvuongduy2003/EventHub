using EventHub.Shared.DTOs.Category;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.Category.CreateCategory;

public class CreateCategoryCommand : IRequest<CategoryDto>
{
    public CreateCategoryCommand(CreateCategoryDto request)
        => (Name, IconImage, Color) = (request.Name, request.IconImage, request.Color);
    
    public string Name { get; set; }
    
    public IFormFile IconImage { get; set; }

    public string Color { get; set; }
}
