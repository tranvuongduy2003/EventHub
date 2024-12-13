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
        
        IQueryable<FavouriteEvent> favouriteEvents = _unitOfWork.FavouriteEvents
            .FindByCondition(x => x.UserId.ToString() == userId);
        
        List<Domain.Aggregates.EventAggregate.Event> events = await _unitOfWork.CachedEvents
            .FindAll()
            .Include(x => x.EventCategories)
                .ThenInclude(x => x.Category)
            .Join(
                favouriteEvents, 
                _event => _event.Id, 
                _favouriteEvent => _favouriteEvent.EventId,
                (_event, _) => _event)
            .ToListAsync(cancellationToken);
        
        List<EventDto> eventDtos = _mapper.Map<List<EventDto>>(events);

        return PagingHelper.Paginate<EventDto>(eventDtos, request.Filter);
    }
}
