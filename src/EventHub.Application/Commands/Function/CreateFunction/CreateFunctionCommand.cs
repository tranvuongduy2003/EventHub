using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Function;

namespace EventHub.Application.Commands.Function.CreateFunction;

/// <summary>
/// Represents a command to create a new function.
/// </summary>
/// <remarks>
/// This command is used to create a new function with the specified details.
/// </remarks>
public class CreateFunctionCommand : ICommand<FunctionDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateFunctionCommand"/> class.
    /// </summary>
    /// <param name="request">
    /// The data transfer object containing the details for the new function.
    /// </param>
    public CreateFunctionCommand(CreateFunctionDto request)
        => (Name, Url, SortOrder, ParentId) =
            (request.Name, request.Url, request.SortOrder, request.ParentId);

    /// <summary>
    /// Gets or sets the name of the new function.
    /// </summary>
    /// <value>
    /// A string representing the name of the function.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL associated with the new function.
    /// </summary>
    /// <value>
    /// A string representing the URL for the function.
    /// </value>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the sort order of the new function.
    /// </summary>
    /// <value>
    /// An integer representing the position or order of the function.
    /// </value>
    public int SortOrder { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the parent function, if any.
    /// </summary>
    /// <value>
    /// A nullable string representing the unique identifier of the parent function.
    /// </value>
    public string? ParentId { get; set; }
}