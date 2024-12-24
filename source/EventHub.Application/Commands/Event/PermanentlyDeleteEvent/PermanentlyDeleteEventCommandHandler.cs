using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Commands.Event.PermanentlyDeleteEvent;

/// <summary>
/// Handles the permanent deletion of events from the system
/// </summary>
public class PermanentlyDeleteEventCommandHandler : ICommandHandler<PermanentlyDeleteEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    /// <summary>
    /// Initializes a new instance of the PermanentlyDeleteEventCommandHandler
    /// </summary>
    /// <param name="unitOfWork">Unit of work for database operations</param>
    /// <param name="fileService">Service for handling file operations</param>
    public PermanentlyDeleteEventCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    /// <summary>
    /// Handles the permanent deletion of an event and all its associated data
    /// </summary>
    /// <param name="request">Command containing the event ID to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <exception cref="NotFoundException">Thrown when the event does not exist</exception>
    public async Task Handle(PermanentlyDeleteEventCommand request, CancellationToken cancellationToken)
    {
        // Get event or throw if not found
        Domain.Aggregates.EventAggregate.Event @event =
            await _unitOfWork.Events.GetDeletedEventByIdAsync(request.EventId, cancellationToken);
        if (@event == null)
        {
            throw new NotFoundException("Event does not exist!");
        }

        // Delete event sub-images and their files
        List<EventSubImage> eventSubImages = await _unitOfWork.EventSubImages
            .FindByCondition(x => x.EventId == @event.Id)
            .ToListAsync(cancellationToken);
        foreach (EventSubImage image in eventSubImages)
        {
            await _fileService.DeleteAsync(FileContainer.EVENTS, image.ImageFileName);
            await _unitOfWork.EventSubImages.Delete(image);
        }

        await _unitOfWork.CommitAsync();

        // Delete email content attachments and their files
        IQueryable<EmailContent> emailContents = _unitOfWork.EmailContents
            .FindByCondition(x => x.EventId == @event.Id)
            .Include(x => x.EmailAttachments);
        var attachments = emailContents.SelectMany(x => x.EmailAttachments).ToList();
        foreach (EmailAttachment attachment in attachments)
        {
            await _fileService.DeleteAsync(FileContainer.EVENTS, attachment.AttachmentFileName);
            await _unitOfWork.EmailAttachments.Delete(attachment);
        }

        await emailContents.ExecuteDeleteAsync(cancellationToken);
        await _unitOfWork.CommitAsync();

        // Delete ticket types
        await _unitOfWork.TicketTypes
            .FindByCondition(x => x.EventId == @event.Id)
            .ExecuteDeleteAsync(cancellationToken);
        await _unitOfWork.CommitAsync();

        // Delete event categories
        await _unitOfWork.EventCategories
            .FindByCondition(x => x.EventId == @event.Id)
            .ExecuteDeleteAsync(cancellationToken);
        await _unitOfWork.CommitAsync();

        // Delete event reasons
        await _unitOfWork.Reasons
            .FindByCondition(x => x.EventId == @event.Id)
            .ExecuteDeleteAsync(cancellationToken);
        await _unitOfWork.CommitAsync();
        
        // Delete event reviews
        await _unitOfWork.Reviews
            .FindByCondition(x => x.EventId == @event.Id)
            .ExecuteDeleteAsync(cancellationToken);
        await _unitOfWork.CommitAsync();

        // Finally delete the event itself
        await _unitOfWork.CachedEvents.Delete(@event);
        await _unitOfWork.CommitAsync();
    }
}
