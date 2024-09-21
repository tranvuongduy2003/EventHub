using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Event.GetEventById;

public class GetEventByIdQueryHandler : IQueryHandler<GetEventByIdQuery, EventDetailDto>
{
    private readonly ILogger<GetEventByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetEventByIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetEventByIdQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<EventDetailDto> Handle(GetEventByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetEventByIdQueryHandler");

        var cachedEvent = await _unitOfWork.CachedEvents
            .FindByCondition(x => x.Id.Equals(request.EventId))
            .Include(x => x.Author)
            .Include(x => x.EmailContent)
            .Include(x => x.Reasons)
            .Include(x => x.TicketTypes)
            .Include(x => x.EventSubImages)
            .ToListAsync();

        if (cachedEvent == null)
            throw new NotFoundException("Event does not exist!");

        var eventDto = _mapper.Map<EventDetailDto>(cachedEvent);

        _logger.LogInformation("END: GetEventByIdQueryHandler");

        return eventDto;
    }
}