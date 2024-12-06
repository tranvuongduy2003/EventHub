#region Usings

using AutoMapper;
using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.Event;
using EventHub.Application.DTOs.File;
using EventHub.Application.Exceptions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Constants;
using EventHub.Domain.Shared.Enums.Event;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

#endregion

namespace EventHub.Application.Commands.Event.CreateEvent;

/// <summary>
/// Handles the creation of new events
/// </summary>
public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand, EventDto>
{
    #region Private Fields

    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly ISerializeService _serializeService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the CreateEventCommandHandler
    /// </summary>
    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
        ISerializeService serializeService, UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _serializeService = serializeService;
        _userManager = userManager;
    }

    #endregion

    #region Handle Method

    /// <summary>
    /// Handles the creation of a new event based on the provided request
    /// </summary>
    /// <param name="request">The create event command containing event details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created event DTO</returns>
    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        // Check if event with same name already exists
        bool isEventExisted = await _unitOfWork.CachedEvents
            .ExistAsync(e => e.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
        if (isEventExisted)
        {
            throw new BadRequestException($"Event '{request.Name}' already existed!");
        }

        // Create and map the event
        Domain.Aggregates.EventAggregate.Event @event = _mapper.Map<Domain.Aggregates.EventAggregate.Event>(request);
        @event.AuthorId = request.AuthorId;

        // Handle cover image upload
        BlobResponseDto coverImage = await _fileService.UploadAsync(request.CoverImage, FileContainer.EVENTS);
        @event.CoverImageUrl = coverImage.Blob.Uri ?? "";
        @event.CoverImageFileName = coverImage.Blob.Name ?? "";

        await _unitOfWork.Events.CreateAsync(@event);
        await _unitOfWork.CommitAsync();

        // Handle sub-images if any
        if (request.EventSubImages != null && request.EventSubImages.Any())
        {
            var eventSubImages = new List<EventSubImage>();
            foreach (IFormFile subImageFile in request.EventSubImages)
            {
                BlobResponseDto subImage =
                    await _fileService.UploadAsync(subImageFile, $"{FileContainer.EVENTS}/{@event.Id}");
                eventSubImages.Add(new EventSubImage
                {
                    EventId = @event.Id,
                    ImageUrl = subImage.Blob.Uri ?? "",
                    ImageFileName = subImage.Blob.Name ?? ""
                });
            }

            await _unitOfWork.EventSubImages.CreateListAsync(eventSubImages);
            await _unitOfWork.CommitAsync();
        }

        // Handle email content if provided
        if (request.EmailContent != null)
        {
            var emailContent = new EmailContent()
            {
                Content = request.EmailContent.Content,
                EventId = @event.Id
            };
            await _unitOfWork.EmailContents.CreateAsync(emailContent);
            await _unitOfWork.CommitAsync();

            // Handle email attachments if any
            if (request.EmailContent.Attachments != null && request.EmailContent.Attachments.Any())
            {
                var emailAttachments = new List<EmailAttachment>();
                foreach (IFormFile attachment in request.EmailContent.Attachments)
                { 
                    BlobResponseDto attachmentFile =
                        await _fileService.UploadAsync(attachment, $"{FileContainer.EVENTS}/{@event.Id}");
                    var emailAttachment = new EmailAttachment()
                    {
                        AttachmentUrl = attachmentFile.Blob.Uri ?? "",
                        AttachmentFileName = attachmentFile.Blob.Name ?? "",
                        EmailContentId = emailContent.Id,
                    };
                    emailAttachments.Add(emailAttachment);
                }

                await _unitOfWork.EmailAttachments.CreateListAsync(emailAttachments);
                await _unitOfWork.CommitAsync();
            }
        }

        // Handle ticket types for paid events
        if (request.EventPaymentType == EEventPaymentType.PAID && request.TicketTypes != null &&
            request.TicketTypes.Any())
        {
            var ticketTypes = request.TicketTypes
                .Select(x =>
                {
                    CreateTicketTypeCommand ticketType = _serializeService.Deserialize<CreateTicketTypeCommand>(x);
                    return new TicketType()
                    {
                        EventId = @event.Id,
                        Name = ticketType.Name,
                        Price = ticketType.Price,
                        Quantity = ticketType.Quantity
                    };
                })
                .ToList();

            await _unitOfWork.TicketTypes.CreateListAsync(ticketTypes);
            await _unitOfWork.CommitAsync();
        }

        // Handle event categories
        if (request.Categories.Any())
        {
            var eventCategories = request.Categories
                .Select(categoryId => new EventCategory
                {
                    CategoryId = categoryId,
                    EventId = @event.Id
                })
                .ToList();

            await _unitOfWork.EventCategories.CreateListAsync(eventCategories);
            await _unitOfWork.CommitAsync();
        }

        // Handle event reasons if any
        if (request.Reasons != null && request.Reasons.Any())
        {
            var reasons = request.Reasons
                .Select(reason => new Reason
                {
                    EventId = @event.Id,
                    Name = reason
                })
                .ToList();

            await _unitOfWork.Reasons.CreateListAsync(reasons);
            await _unitOfWork.CommitAsync();
        }

        // Update user's created events count
        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(request.AuthorId.ToString());
        if (user != null)
        {
            user.NumberOfCreatedEvents++;
            await _userManager.UpdateAsync(user);
        }

        // Map and return the event DTO
        EventDto eventDto = _mapper.Map<EventDto>(@event);
        return eventDto;
    }

    #endregion
}
