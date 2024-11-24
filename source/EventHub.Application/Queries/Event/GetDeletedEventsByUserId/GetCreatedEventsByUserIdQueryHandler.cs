using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Event.GetDeletedEventsByUserId;

public class GetDeletedEventsByUserIdQueryHandler : IQueryHandler<GetDeletedEventsByUserIdQuery, Pagination<EventDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetDeletedEventsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<EventDto>> Handle(GetDeletedEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        List<Domain.AggregateModels.EventAggregate.Event> cachedEvents = await _unitOfWork.CachedEvents
            .FindByCondition(x => x.AuthorId.Equals(request.userId) && x.IsDeleted)
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
                Domain.AggregateModels.EventAggregate.Event @event = group.Key;
                @event.Categories = group.Select(g => g.EventCategory.Category).ToList();
                return @event;
            })
            .ToList();

        List<EventDto> eventDtos = _mapper.Map<List<EventDto>>(events);

        return PagingHelper.Paginate<EventDto>(eventDtos, request.Filter);
    }
}
