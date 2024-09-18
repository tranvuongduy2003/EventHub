using AutoMapper;
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.Enums.Event;
using EventHub.Shared.Exceptions;
using EventHub.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Event.CreateEvent;

public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand, EventDto>
{
    private readonly IFileService _fileService;
    private readonly ILogger<CreateEventCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ISerializeService _serializeService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
        ILogger<CreateEventCommandHandler> logger, ISerializeService serializeService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _logger = logger;
        _serializeService = serializeService;
    }

    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: CreateEventCommandHandler");

        var isEventExisted = await _unitOfWork.CachedEvents
            .ExistAsync(e => e.Name.Equals(request.Event.Name, StringComparison.OrdinalIgnoreCase));
        if (isEventExisted)
            throw new BadRequestException($"Event '{request.Event.Name}' already existed!");

        var @event = _mapper.Map<Domain.AggregateModels.EventAggregate.Event>(request.Event);

        @event.AuthorId = request.AuthorId;

        var coverImage = await _fileService.UploadAsync(request.Event.CoverImage, FileContainer.EVENTS);
        @event.CoverImageUrl = coverImage.Blob.Uri ?? "";
        @event.CoverImageFileName = coverImage.Blob.Name ?? "";

        await _unitOfWork.Events.CreateAsync(@event);
        await _unitOfWork.CommitAsync();

        if (request.Event.EventSubImages != null && request.Event.EventSubImages.Any())
        {
            await Domain.AggregateModels.EventAggregate.Event
                .UploadAndSaveEventSubImages(@event.Id, request.Event.EventSubImages);
        }

        if (request.Event.EmailContent != null)
        {
            await Domain.AggregateModels.EventAggregate.Event
                .CreateEmailContentOfEvent(@event.Id, request.Event.EmailContent);
        }

        if (request.Event.EventPaymentType == EEventPaymentType.PAID && request.Event.TicketTypes != null &&
            request.Event.TicketTypes.Any())
        {
            var ticketTypes = request.Event.TicketTypes
                .Select(x => _serializeService.Deserialize<CreateTicketTypeDto>(x))
                .ToList();
            await Domain.AggregateModels.EventAggregate.Event
                .CreateTicketTypesOfEvent(@event.Id, ticketTypes);
        }

        if (request.Event.Categories.Any())
        {
            await Domain.AggregateModels.EventAggregate.Event
                .AddEventToCategories(@event.Id, request.Event.Categories);
        }

        if (request.Event.Reasons != null && request.Event.Reasons.Any())
        {
            await Domain.AggregateModels.EventAggregate.Event
                .CreateReasonsToRegisterEvent(@event.Id, request.Event.Reasons.ToList());
        }

        var eventDto = _mapper.Map<EventDto>(@event);

        return eventDto;
    }
}