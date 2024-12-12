using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Event.GetFavouriteEventsByUserId;

public class
    GetFavouriteEventsByUserIdQueryHandler : IQueryHandler<GetFavouriteEventsByUserIdQuery,
        Pagination<EventDto>>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;

    public GetFavouriteEventsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public async Task<Pagination<EventDto>> Handle(GetFavouriteEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";
        
        List<Domain.Aggregates.EventAggregate.Event> cachedEvents = await _unitOfWork.CachedEvents
            .FindByCondition(x => x.AuthorId.ToString() == userId)
            .ToListAsync(cancellationToken);
        List<EventCategory> eventCategories = await _unitOfWork.EventCategories
            .FindAll()
            .Include(x => x.Category)
            .ToListAsync(cancellationToken);
        List<FavouriteEvent> favouriteEvents = await _unitOfWork.FavouriteEvents
            .FindByCondition(x => x.UserId.ToString() == userId)
            .ToListAsync(cancellationToken);

        var events = (
                from _event in cachedEvents
                join _favouriteEvent in favouriteEvents
                    on _event.Id equals _favouriteEvent.EventId
                join _eventCategory in eventCategories.DefaultIfEmpty()
                    on _event.Id equals _eventCategory.EventId
                group _eventCategory.Category by _event
                into g
                select new { Event = g.Key, Categories = g.ToList() })
            .AsEnumerable()
            .Select(x =>
            {
                x.Event.Categories = x.Categories;
                return x.Event;
            })
            .ToList();

        List<EventDto> eventDtos = _mapper.Map<List<EventDto>>(events);

        return PagingHelper.Paginate<EventDto>(eventDtos, request.Filter);
    }
}
