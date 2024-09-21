using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Category;

namespace EventHub.Application.Commands.Category.UpdateCategory;

/// <summary>
/// Represents a command to update an existing category.
/// </summary>
/// <remarks>
/// This command is used to request an update to an existing category identified by its unique identifier.
/// </remarks>
public class UpdateCategoryCommand : ICommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCategoryCommand"/> class.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the category to be updated.
    /// </param>
    /// <param name="request">
    /// The data transfer object containing the updated details of the category.
    /// </param>
    public UpdateCategoryCommand(Guid id, UpdateCategoryDto request)
        => (Id, Category) = (id, request);

    /// <summary>
    /// Gets or sets the unique identifier of the category to be updated.
    /// </summary>
    /// <value>
    /// A string representing the unique identifier of the category.
    /// </value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the data transfer object containing the updated details of the category.
    /// </summary>
    /// <value>
    /// An <see cref="UpdateCategoryDto"/> instance with the new details for the category.
    /// </value>
    public UpdateCategoryDto Category { get; set; }
}