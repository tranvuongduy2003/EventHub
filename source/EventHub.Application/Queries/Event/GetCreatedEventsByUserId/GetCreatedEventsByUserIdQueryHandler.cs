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
                query => query
                    .Include(x => x.EventCategories)
                        .ThenInclude(x => x.Category)
                    .Include(x => x.EventCoupons)
                        .ThenInclude(x => x.Coupon));

        Pagination<EventDto> paginatedEventDtos = _mapper.Map<Pagination<EventDto>>(paginatedEvents);

        return Task.FromResult(paginatedEventDtos);
    }
}
