using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.Event.UpdateEvent;

/// <summary>
/// Represents a command to update email content for an event.
/// </summary>
public class UpdateEmailContentCommand : ICommand<UpdateEmailContentDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEmailContentCommand"/> class.
    /// </summary>
    public UpdateEmailContentCommand()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEmailContentCommand"/> class.
    /// </summary>
    /// <param name="request">The DTO containing email content update details.</param>
    public UpdateEmailContentCommand(UpdateEmailContentDto request)
    {
        Id = request.Id;
        Content = request.Content;
        Attachments = request.Attachments;
    }

    /// <summary>
    /// Gets or sets the unique identifier of the email content.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the main content of the email.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets optional attachments to be included in the email.
    /// </summary>
    public IFormFileCollection? Attachments { get; set; }
}
