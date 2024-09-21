using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Event.GetFavouriteEventsByUserId;

public class
    GetFavouriteEventsByUserIdQueryHandler : IQueryHandler<GetFavouriteEventsByUserIdQuery,
        Pagination<EventDto>>
{
    private readonly ILogger<GetFavouriteEventsByUserIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetFavouriteEventsByUserIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetFavouriteEventsByUserIdQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<EventDto>> Handle(GetFavouriteEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetFavouriteEventsByUserIdQueryHandler");

        var cachedEvents = _unitOfWork.CachedEvents
            .FindByCondition(x => x.AuthorId.Equals(request.userId));
        var eventCategories = _unitOfWork.EventCategories
            .FindAll()
            .Include(x => x.Category);
        var favouriteEvents = _unitOfWork.FavouriteEvents
            .FindByCondition(x => x.UserId.Equals(request.userId));

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

        var eventDtos = _mapper.Map<List<EventDto>>(events);

        _logger.LogInformation("END: GetFavouriteEventsByUserIdQueryHandler");

        return PagingHelper.Paginate<EventDto>(eventDtos, request.Filter);
    }
}