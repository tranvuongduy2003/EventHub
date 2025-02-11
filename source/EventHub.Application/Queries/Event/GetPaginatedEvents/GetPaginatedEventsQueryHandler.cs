using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
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

    public Task<Pagination<EventDto>> Handle(GetPaginatedEventsQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.EventAggregate.Event> paginatedEvents = _unitOfWork.CachedEvents
            .PaginatedFind(
                request.Filter,
                query =>
                {
                    if (request.Filter.DateRange != null)
                    {
                        query = query.Where(x => request.Filter.DateRange.StartDate <= x.StartTime &&
                                                 x.EndTime <= request.Filter.DateRange.EndDate);
                    }

                    if (request.Filter.Status != null)
                    {
                        query = request.Filter.Status switch
                        {
                            Domain.Shared.Enums.Event.EEventStatus.OPENING =>
                                query.Where(x => DateTime.UtcNow >= x.StartTime && DateTime.UtcNow <= x.EndTime),

                            Domain.Shared.Enums.Event.EEventStatus.UPCOMING =>
                                query.Where(x => DateTime.UtcNow < x.StartTime),

                            Domain.Shared.Enums.Event.EEventStatus.CLOSED =>
                                query.Where(x => DateTime.UtcNow > x.EndTime),

                            _ => query
                        };
                    }

                    query = query.Include(x => x.Reviews);
                    if (request.Filter.Rate is int rate && rate is >= 1 and <= 5)
                    {
                        query = query.Where(x => x.Reviews.Average(r => r.Rate) >= rate);
                    }

                    query = query.Include(x => x.EventCategories).ThenInclude(x => x.Category);
                    if (request.Filter.CategoryIds?.Any() == true)
                    {
                        var categorySet = new HashSet<Guid>(request.Filter.CategoryIds);
                        query = query.Where(x =>
                            x.EventCategories.Any(ec => categorySet.Contains(ec.CategoryId)));
                    }

                    return query
                        .Include(x => x.EventCoupons).ThenInclude(x => x.Coupon)
                        .Include(x => x.TicketTypes)
                        .Include(x => x.Expenses);
                }
            );

        Pagination<EventDto> paginatedEventDtos = _mapper.Map<Pagination<EventDto>>(paginatedEvents);

        for (int i = 0; i < paginatedEventDtos.Items.Count; i++)
        {
            paginatedEventDtos.Items[i].AverageRate = paginatedEvents.Items[i].Reviews != null && paginatedEvents.Items[i].Reviews.Any() ? Math.Round(paginatedEvents.Items[i].Reviews.Average(x => x.Rate), 2) : 0.00;
        }

        return Task.FromResult(paginatedEventDtos);
    }
}
