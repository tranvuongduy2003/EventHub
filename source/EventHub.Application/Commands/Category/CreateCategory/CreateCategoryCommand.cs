using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Category;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.Category.CreateCategory;

/// <summary>
/// Represents a command to create a new category.
/// </summary>
/// <remarks>
/// This command is used to create a new category with the specified details.
/// </remarks>
public class CreateCategoryCommand : ICommand<CategoryDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCategoryCommand"/> class.
    /// </summary>
    /// <param name="request">
    /// The data transfer object containing the details for the new category.
    /// </param>
    public CreateCategoryCommand(CreateCategoryDto request)
        => (Name, IconImage, Color) = (request.Name, request.IconImage, request.Color);

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