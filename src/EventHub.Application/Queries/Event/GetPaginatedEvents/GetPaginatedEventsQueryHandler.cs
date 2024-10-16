using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Event.GetPaginatedEvents;

public class GetPaginatedEventsQueryHandler : IQueryHandler<GetPaginatedEventsQuery, Pagination<EventDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedEventsQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<EventDto>> Handle(GetPaginatedEventsQuery request,
        CancellationToken cancellationToken)
    {

        var cachedEvents = _unitOfWork.CachedEvents.FindAll();
        var eventCategories = _unitOfWork.EventCategories
            .FindAll()
            .Include(x => x.Category);

        var events = cachedEvents
            .LeftJoin(
                eventCategories,
                _event => _event.Id,
                _eventCategory => _eventCategory.EventId,
                (_event, _eventCategory) => new { Event = _event, EventCategory = _eventCategory })
            .GroupBy(x => x.Event)
            .AsEnumerable()
            .Select(group =>
            {
                var @event = group.Key;
                @event.Categories = group.Select(g => g.EventCategory.Category).ToList();
                return @event;
            })
            .ToList();

        var eventDtos = _mapper.Map<List<EventDto>>(events);


        return PagingHelper.Paginate<EventDto>(eventDtos, request.Filter);
    }
}