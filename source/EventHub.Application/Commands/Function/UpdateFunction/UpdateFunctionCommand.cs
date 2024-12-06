using EventHub.Application.DTOs.Function;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Function.UpdateFunction;

/// <summary>
/// Represents a command to update an existing function.
/// </summary>
/// <remarks>
/// This command is used to request an update to an existing function specified by its unique identifier.
/// </remarks>
public class UpdateFunctionCommand : ICommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateFunctionCommand"/> class.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the function to be updated.
    /// </param>
    /// <param name="request">
    /// The data transfer object containing the updated details of the function.
    /// </param>
    public UpdateFunctionCommand(string id, UpdateFunctionDto request)
    {
        Id = id;
        Name = request.Name;
        Url = request.Url;
        SortOrder = request.SortOrder;
        ParentId = request.ParentId;
    }

    /// <summary>
    /// Gets or sets the unique identifier of the function to be updated.
    /// </summary>
    /// <value>
    /// A string representing the unique identifier of the function.
    /// </value>
    public string Id { get; set; }

    public string Name { get; set; }

    public string Url { get; set; }

    public int SortOrder { get; set; }

    public string? ParentId { get; set; }
}
