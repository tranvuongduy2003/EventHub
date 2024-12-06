using AutoMapper;
using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.Event;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Event.GetPaginatedEvents;

public class GetPaginatedEventsQueryHandler : IQueryHandler<GetPaginatedEventsQuery, Pagination<EventDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedEventsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<EventDto>> Handle(GetPaginatedEventsQuery request,
        CancellationToken cancellationToken)
    {
        List<Domain.Aggregates.EventAggregate.Event> cachedEvents = await _unitOfWork.CachedEvents.FindAll()
            .ToListAsync(cancellationToken);
        List<EventCategory> eventCategories = await _unitOfWork.EventCategories
            .FindAll()
            .Include(x => x.Category)
            .ToListAsync(cancellationToken);

        var events = (
                from _event in cachedEvents
                join _eventCategory in eventCategories.DefaultIfEmpty()
                    on _event.Id equals _eventCategory.EventId
                select new { Event = _event, EventCategory = _eventCategory }
            )
            .GroupBy(x => x.Event)
            .AsEnumerable()
            .Select(group =>
            {
                Domain.Aggregates.EventAggregate.Event @event = group.Key;
                @event.Categories = group.Select(g => g.EventCategory.Category).ToList();
                return @event;
            })
            .ToList();

        List<EventDto> eventDtos = _mapper.Map<List<EventDto>>(events);

        return PagingHelper.Paginate<EventDto>(eventDtos, request.Filter);
    }
}
