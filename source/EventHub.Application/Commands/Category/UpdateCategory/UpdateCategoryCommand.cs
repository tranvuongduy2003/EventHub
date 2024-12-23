using EventHub.Application.SeedWork.DTOs.Category;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Http;

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
    public UpdateCategoryCommand()
    {
    }

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
        => (Id, Name, IconImage, Color) = (id, request.Name, request.IconImage, request.Color);

    /// <summary>
    /// Gets or sets the unique identifier of the category to be updated.
    /// </summary>
    /// <value>
    /// A string representing the unique identifier of the category.
    /// </value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the new category.
    /// </summary>
    /// <value>
    /// A string representing the name of the category.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the icon image file for the new category.
    /// </summary>
    /// <value>
    /// An <see cref="IFormFile"/> representing the icon image of the category.
    /// </value>
    public IFormFile IconImage { get; set; }

    /// <summary>
    /// Gets or sets the color associated with the new category.
    /// </summary>
    /// <value>
    /// A string representing the color associated with the category.
    /// </value>
    public string Color { get; set; }
}
