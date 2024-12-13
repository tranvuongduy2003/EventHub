using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
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
        List<Domain.Aggregates.EventAggregate.Event> events = await _unitOfWork.CachedEvents
            .FindAll()
            .Include(x => x.EventCategories)
                .ThenInclude(x => x.Category)
            .ToListAsync(cancellationToken);

        List<EventDto> eventDtos = _mapper.Map<List<EventDto>>(events);

        return PagingHelper.Paginate<EventDto>(eventDtos, request.Filter);
    }
}
