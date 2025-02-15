﻿using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Event.GetCreatedEventsByUserId;

public class GetCreatedEventsByUserIdQueryHandler : IQueryHandler<GetCreatedEventsByUserIdQuery, Pagination<EventDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public GetCreatedEventsByUserIdQueryHandler(IUnitOfWork unitOfWork,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public Task<Pagination<EventDto>> Handle(GetCreatedEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()
            ?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "");

        Pagination<Domain.Aggregates.EventAggregate.Event> paginatedEvents = _unitOfWork.CachedEvents
            .PaginatedFindByCondition(
                x => x.AuthorId == userId,
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

                    if (request.Filter.Rate is int rate && rate is >= 1 and <= 5)
                    {
                        query = query.Where(x => (x.Reviews.Any() ? x.Reviews.Average(r => r.Rate) : 0) >= rate);
                    }

                    if (request.Filter.CategoryIds?.Any() == true)
                    {
                        var categorySet = new HashSet<Guid>(request.Filter.CategoryIds);
                        query = query.Where(x => x.EventCategories.Any(ec => categorySet.Contains(ec.CategoryId)));
                    }

                    if (request.Filter.Visibility != null || request.Filter.Visibility != Domain.Shared.Enums.Event.EVisibility.ALL)
                    {
                        query = request.Filter.Visibility switch
                        {
                            Domain.Shared.Enums.Event.EVisibility.PRIVATE =>
                                query.Where(x => x.IsPrivate),

                            Domain.Shared.Enums.Event.EVisibility.PUBLIC =>
                                query.Where(x => !x.IsPrivate),

                            _ => query
                        };
                    }

                    return query
                        .Include(x => x.EventCategories).ThenInclude(x => x.Category)
                        .Include(x => x.EventCoupons).ThenInclude(x => x.Coupon)
                        .Include(x => x.Expenses)
                        .Include(x => x.Reviews);
                });

        Pagination<EventDto> paginatedEventDtos = _mapper.Map<Pagination<EventDto>>(paginatedEvents);

        for (int i = 0; i < paginatedEventDtos.Items.Count; i++)
        {
            paginatedEventDtos.Items[i].AverageRate = paginatedEvents.Items[i].Reviews != null && paginatedEvents.Items[i].Reviews.Any() ? Math.Round(paginatedEvents.Items[i].Reviews.Average(x => x.Rate), 2) : 0.00;
        }

        return Task.FromResult(paginatedEventDtos);
    }
}
