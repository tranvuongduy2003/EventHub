using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.File;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Enums.Event;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Commands.Event.UpdateEvent;

public class UpdateEventCommandHandler : ICommandHandler<UpdateEventCommand>
{
    private readonly IFileService _fileService;
    private readonly ISerializeService _serializeService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEventCommandHandler(IUnitOfWork unitOfWork, IFileService fileService,
        ISerializeService serializeService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _serializeService = serializeService;
    }

    public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.Event @event = await ValidateEvent(request.EventId);

        @event = await UpdateEventProperties(@event, request);

        await UpdateEventSubImages(@event.Id, request.EventSubImages, cancellationToken);

        if (request.EmailContent != null)
        {
            await UpdateEmailContent(@event.Id, request.EmailContent, cancellationToken);
        }

        if (@event.EventPaymentType == EEventPaymentType.PAID && request.TicketTypes?.Any() == true)
        {
            await UpdateTicketTypes(@event.Id, request.TicketTypes);
        }

        if (request.Categories.Any())
        {
            await UpdateCategories(@event.Id, request.Categories);
        }

        if (request.Reasons?.Any() == true)
        {
            await UpdateReasons(@event.Id, request.Reasons);
        }

        await _unitOfWork.CommitAsync();
    }

    private async Task<Domain.Aggregates.EventAggregate.Event> ValidateEvent(Guid eventId)
    {
        Domain.Aggregates.EventAggregate.Event @event = await _unitOfWork.CachedEvents.GetByIdAsync(eventId);
        if (@event == null)
        {
            throw new NotFoundException("Event does not exist!");
        }
        bool isSameNameEventExisted = await _unitOfWork.CachedEvents
            .ExistAsync(e => e.Name.Equals(@event.Name, StringComparison.OrdinalIgnoreCase));
        if (isSameNameEventExisted)
        {
            throw new BadRequestException($"Event '{@event.Name}' already existed!");
        }

        return @event;
    }

    private async Task<Domain.Aggregates.EventAggregate.Event> UpdateEventProperties(Domain.Aggregates.EventAggregate.Event @event, UpdateEventCommand request)
    {
        @event.Name = request.Name;
        @event.Description = request.Description;
        @event.Location = request.Location;
        @event.StartTime = request.StartTime;
        @event.EndTime = request.EndTime;
        @event.Promotion = request.Promotion;
        @event.EventCycleType = request.EventCycleType;
        @event.EventPaymentType = request.EventPaymentType;
        @event.IsPrivate = request.IsPrivate;

        await _fileService.DeleteAsync(FileContainer.EVENTS, @event.CoverImageFileName);
        if (request.CoverImage != null)
        {
            BlobResponseDto coverImageResponse = await _fileService.UploadAsync(request.CoverImage, FileContainer.EVENTS);
            @event.CoverImageUrl = coverImageResponse.Blob.Uri ?? "";
            @event.CoverImageFileName = coverImageResponse.Blob.Name ?? "";
        }

        await _unitOfWork.Events.Update(@event);
        await _unitOfWork.CommitAsync();

        return @event;
    }

    private async Task UpdateEventSubImages(Guid eventId, IFormFileCollection? subImages, CancellationToken cancellationToken)
    {
        List<EventSubImage> eventSubImages = await _unitOfWork.EventSubImages
            .FindByCondition(x => x.EventId.Equals(eventId))
            .ToListAsync(cancellationToken);

        foreach (EventSubImage image in eventSubImages)
        {
            await _fileService.DeleteAsync($"{FileContainer.EVENTS}/{eventId}", image.ImageFileName);
        }

        if (subImages?.Any() == true)
        {
            var updatedSubImages = new List<EventSubImage>();
            foreach (IFormFile subImageFile in subImages)
            {
                BlobResponseDto subImage =
                    await _fileService.UploadAsync(subImageFile, $"{FileContainer.EVENTS}/{eventId}");
                updatedSubImages.Add(new EventSubImage
                {
                    EventId = eventId,
                    ImageUrl = subImage.Blob.Uri ?? "",
                    ImageFileName = subImage.Blob.Name ?? ""
                });
            }

            await _unitOfWork.EventSubImages.CreateListAsync(eventSubImages);
            await _unitOfWork.CommitAsync();
        }

        await _unitOfWork.CommitAsync();
    }

    private async Task UpdateEmailContent(Guid eventId, UpdateEmailContentCommand emailContentCommand, CancellationToken cancellationToken)
    {
        EmailContent emailContent = await _unitOfWork.EmailContents.GetByIdAsync(emailContentCommand.Id);
        if (emailContent == null)
        {
            throw new NotFoundException("EmailContent does not exist!");
        }

        emailContent.Content = emailContentCommand.Content;

        List<EmailAttachment> attachments = await _unitOfWork.EmailAttachments
            .FindByCondition(x => x.EmailContentId.Equals(emailContent.Id))
            .ToListAsync(cancellationToken);
        foreach (EmailAttachment attachment in attachments)
        {
            await _fileService.DeleteAsync($"{FileContainer.EVENTS}/{eventId}",
                attachment.AttachmentFileName);
        }

        if (emailContentCommand.Attachments != null && emailContentCommand.Attachments.Any())
        {
            var emailAttachments = new List<EmailAttachment>();
            foreach (IFormFile attachment in emailContentCommand.Attachments)
            {
                BlobResponseDto attachmentFile =
                    await _fileService.UploadAsync(attachment, $"{FileContainer.EVENTS}/{eventId}");
                var emailAttachment = new EmailAttachment()
                {
                    AttachmentUrl = attachmentFile.Blob.Uri ?? "",
                    AttachmentFileName = attachmentFile.Blob.Name ?? "",
                    EmailContentId = emailContent.Id,
                };
                emailAttachments.Add(emailAttachment);
            }

            await _unitOfWork.EmailAttachments.CreateListAsync(emailAttachments);
        }

        await _unitOfWork.EmailContents.Update(emailContent);
        await _unitOfWork.CommitAsync();
    }

    private async Task UpdateTicketTypes(Guid eventId, IEnumerable<string> ticketTypes)
    {
        var deserializedTicketTypes = ticketTypes
            .Select(x => _serializeService.Deserialize<UpdateTicketTypeCommand>(x))
            .ToList();

        var createdTicketTypes = new List<TicketType>();
        foreach (UpdateTicketTypeCommand ticketType in deserializedTicketTypes)
        {
            if (ticketType.Id != null)
            {
                TicketType existingTicketType = await _unitOfWork.TicketTypes.GetByIdAsync((Guid)ticketType.Id);
                existingTicketType.Name = ticketType.Name;
                existingTicketType.Quantity = ticketType.Quantity;
                existingTicketType.Price = ticketType.Price;
                await _unitOfWork.TicketTypes.Update(existingTicketType);
            }
            else
            {
                createdTicketTypes.Add(new TicketType
                {
                    EventId = eventId,
                    Name = ticketType.Name,
                    Price = ticketType.Price,
                    Quantity = ticketType.Quantity
                });
            }
        }

        if (createdTicketTypes.Any())
        {
            await _unitOfWork.TicketTypes.CreateListAsync(createdTicketTypes);
        }

        await _unitOfWork.CommitAsync();
    }

    private async Task UpdateCategories(Guid eventId, IEnumerable<Guid> categories)
    {
        IQueryable<EventCategory> deletedEventCategories = _unitOfWork.EventCategories
            .FindByCondition(x => x.EventId.Equals(eventId));
        await _unitOfWork.EventCategories.DeleteList(deletedEventCategories);

        var eventCategories = new List<EventCategory>();
        foreach (Guid categoryId in categories)
        {
            eventCategories.Add(new EventCategory()
            {
                CategoryId = categoryId,
                EventId = eventId
            });
        }

        await _unitOfWork.EventCategories.CreateListAsync(eventCategories);
        await _unitOfWork.CommitAsync();
    }

    private async Task UpdateReasons(Guid eventId, IEnumerable<string> reasons)
    {
        IQueryable<Reason> deletedReasons = _unitOfWork.Reasons
            .FindByCondition(x => x.EventId.Equals(eventId));
        await _unitOfWork.Reasons.DeleteList(deletedReasons);

        var updatedReasons = reasons
            .Select(reason => new Reason
            {
                EventId = eventId,
                Name = reason
            })
            .ToList();

        await _unitOfWork.Reasons.CreateListAsync(updatedReasons);
        await _unitOfWork.CommitAsync();
    }
}
