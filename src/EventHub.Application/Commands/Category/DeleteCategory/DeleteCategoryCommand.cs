using MediatR;

namespace EventHub.Application.Commands.Category.DeleteCategory;

/// <summary>
/// Represents a command to delete a category by its identifier.
/// </summary>
/// <remarks>
/// This command is used to request the deletion of a category specified by its unique identifier.
/// </remarks>
public record DeleteCategoryCommand(Guid Id) : IRequest;