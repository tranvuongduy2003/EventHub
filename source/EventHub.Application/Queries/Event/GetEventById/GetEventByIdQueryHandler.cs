using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Event.GetEventById;

public class GetEventByIdQueryHandler : IQueryHandler<GetEventByIdQuery, EventDetailDto>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;

    public GetEventByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public async Task<EventDetailDto> Handle(GetEventByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.Event cachedEvent = await _unitOfWork.CachedEvents
            .FindByCondition(x => x.Id == request.EventId)
            .Include(x => x.EmailContent)
                .ThenInclude(x => x != null ? x.EmailAttachments : default)
            .Include(x => x.EventCategories)
                .ThenInclude(x => x.Category)
            .Include(x => x.EventCoupons)
                .ThenInclude(x => x.Coupon)
            .Include(x => x.Author)
            .Include(x => x.Reasons)
            .Include(x => x.TicketTypes)
            .Include(x => x.EventSubImages)
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(cancellationToken);

        if (cachedEvent == null)
        {
            throw new NotFoundException("Event does not exist!");
        }

        EventDetailDto eventDto = _mapper.Map<EventDetailDto>(cachedEvent);

        eventDto.AverageRate = cachedEvent.Reviews != null && cachedEvent.Reviews.Any()
            ? Math.Round(cachedEvent.Reviews.Average(x => x.Rate), 2)
            : 0.00;

        string userId = _signInManager.Context.User.Identities.FirstOrDefault()
            ?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";
        if (!string.IsNullOrEmpty(userId))
        {
            eventDto.IsFavourite =
                await _unitOfWork.FavouriteEvents.ExistAsync(x =>
                    x.EventId == request.EventId && x.UserId == Guid.Parse(userId));

            eventDto.Author.IsFollower =
                await _unitOfWork.UserFollowers.ExistAsync(x =>
                    x.FollowerId == Guid.Parse(userId) && x.FollowedId == Guid.Parse(eventDto.Author.Id));
        }

        return eventDto;
    }
}
