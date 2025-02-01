using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Event.GetFavouriteEventsByUserId;

public class GetFavouriteEventsByUserIdQueryHandler : IQueryHandler<GetFavouriteEventsByUserIdQuery, Pagination<EventDto>>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;

    public GetFavouriteEventsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public Task<Pagination<EventDto>> Handle(GetFavouriteEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        IQueryable<FavouriteEvent> favouriteEvents = _unitOfWork.FavouriteEvents
            .FindByCondition(x => x.UserId.ToString() == userId);

        Pagination<Domain.Aggregates.EventAggregate.Event> paginatedEvents = _unitOfWork.CachedEvents.PaginatedFind(
            request.Filter,
            query =>
            {
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

                if (request.Filter.Rate is int rate && rate is >= 1 and <= 5)
                {
                    query = query.Where(x => x.Reviews.Average(r => r.Rate) >= rate);
                }

                if (request.Filter.CategoryIds?.Any() == true)
                {
                    var categorySet = new HashSet<Guid>(request.Filter.CategoryIds);
                    query = query.Where(x => x.EventCategories.Any(ec => categorySet.Contains(ec.CategoryId)));
                }

                return query
                    .Include(x => x.EventCategories).ThenInclude(x => x.Category)
                    .Include(x => x.EventCoupons).ThenInclude(x => x.Coupon)
                    .Include(x => x.Reviews)
                    .Join(
                        favouriteEvents,
                        _event => _event.Id,
                        _favouriteEvent => _favouriteEvent.EventId,
                        (_event, _) => _event);
            });

        Pagination<EventDto> paginatedEventDtos = _mapper.Map<Pagination<EventDto>>(paginatedEvents);

        for (int i = 0; i < paginatedEventDtos.Items.Count; i++)
        {
            paginatedEventDtos.Items[i].AverageRate = paginatedEvents.Items[i].Reviews != null && paginatedEvents.Items[i].Reviews.Any() ? Math.Round(paginatedEvents.Items[i].Reviews.Average(x => x.Rate), 2) : 0.00;
        }

        return Task.FromResult(paginatedEventDtos);
    }
}
