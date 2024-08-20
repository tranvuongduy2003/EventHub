using MediatR;

namespace EventHub.Application.Commands.Category.DeleteCategory;

public record DeleteCategoryCommand(string Id) : IRequest;