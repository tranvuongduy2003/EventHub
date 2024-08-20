using EventHub.Shared.DTOs.Category;
using MediatR;

namespace EventHub.Application.Commands.Category.UpdateCategory;

public class UpdateCategoryCommand : IRequest
{
    public UpdateCategoryCommand(string id, UpdateCategoryDto request)
        => (Id, Category) = (id, request);
    
    public string Id { get; set; }

    public UpdateCategoryDto Category { get; set; }
}