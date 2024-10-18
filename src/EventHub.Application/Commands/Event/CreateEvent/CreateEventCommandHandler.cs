using AutoMapper;
using EventHub.Abstractions;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Abstractions.Services;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.Enums.Event;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Event.CreateEvent;

public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand, EventDto>
{
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly ISerializeService _serializeService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService,
        ISerializeService serializeService, UserManager<Domain.AggregateModels.UserAggregate.User> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _serializeService = serializeService;
        _userManager = userManager;
    }

    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
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

        var user = await _userManager.FindByIdAsync(request.AuthorId.ToString());
        if (user != null)
        {
            user.NumberOfCreatedEvents++;
            await _userManager.UpdateAsync(user);
        }

        var eventDto = _mapper.Map<EventDto>(@event);

        return eventDto;
    }
}