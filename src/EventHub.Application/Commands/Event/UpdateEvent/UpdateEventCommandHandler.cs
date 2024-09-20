using AutoMapper;
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.Enums.Event;
using EventHub.Shared.Exceptions;
using EventHub.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Event.UpdateEvent;

public class UpdateEventCommandHandler : ICommandHandler<UpdateEventCommand>
{
    private readonly IFileService _fileService;
    private readonly ILogger<UpdateEventCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ISerializeService _serializeService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
        ILogger<UpdateEventCommandHandler> logger, ISerializeService serializeService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _logger = logger;
        _serializeService = serializeService;
    }

    public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: UpdateEventCommandHandler");

        var @event = await _unitOfWork.CachedEvents.GetByIdAsync(request.EventId);
        if (@event == null)
            throw new NotFoundException("Event does not exist!");

        var isSameNameEventExisted = await _unitOfWork.CachedEvents
            .ExistAsync(e => e.Name.Equals(request.Event.Name, StringComparison.OrdinalIgnoreCase));
        if (isSameNameEventExisted)
            throw new BadRequestException($"Event '{request.Event.Name}' already existed!");

        @event.Name = request.Event.Name;
        @event.Description = request.Event.Description;
        @event.Location = request.Event.Location;
        @event.StartTime = request.Event.StartTime;
        @event.EndTime = request.Event.EndTime;
        @event.Promotion = request.Event.Promotion;
        @event.EventCycleType = request.Event.EventCycleType;
        @event.EventPaymentType = request.Event.EventPaymentType;
        @event.IsPrivate = request.Event.IsPrivate;

        await _fileService.DeleteAsync(FileContainer.EVENTS, @event.CoverImageFileName);
        var coverImage = await _fileService.UploadAsync(request.Event.CoverImage, FileContainer.EVENTS);
        @event.CoverImageUrl = coverImage.Blob.Uri ?? "";
        @event.CoverImageFileName = coverImage.Blob.Name ?? "";

        await _unitOfWork.Events.UpdateAsync(@event);
        await _unitOfWork.CommitAsync();

        if (request.Event.EventSubImages != null && request.Event.EventSubImages.Any())
        {
            await Domain.AggregateModels.EventAggregate.Event
                .DeleteEventSubImages(@event.Id);
            await Domain.AggregateModels.EventAggregate.Event
                .UploadAndSaveEventSubImages(@event.Id, request.Event.EventSubImages);
        }

        if (request.Event.EmailContent != null)
        {
            await Domain.AggregateModels.EventAggregate.Event
                .UpdateEmailContentOfEvent(@event.Id, request.Event.EmailContent);
        }

        if (request.Event.EventPaymentType == EEventPaymentType.PAID && request.Event.TicketTypes != null &&
            request.Event.TicketTypes.Any())
        {
            var ticketTypes = request.Event.TicketTypes
                .Select(x => _serializeService.Deserialize<UpdateTicketTypeDto>(x))
                .ToList();
            await Domain.AggregateModels.EventAggregate.Event
                .UpdateTicketTypesInEvent(@event.Id, ticketTypes);
        }

        if (request.Event.Categories.Any())
        {
            await Domain.AggregateModels.EventAggregate.Event
                .UpdateCategoriesInEvent(@event.Id, request.Event.Categories);
        }

        if (request.Event.Reasons != null && request.Event.Reasons.Any())
        {
            await Domain.AggregateModels.EventAggregate.Event
                .UpdateReasonsInEvent(@event.Id, request.Event.Reasons.ToList());
        }

        _logger.LogInformation("END: UpdateEventCommandHandler");
    }
}