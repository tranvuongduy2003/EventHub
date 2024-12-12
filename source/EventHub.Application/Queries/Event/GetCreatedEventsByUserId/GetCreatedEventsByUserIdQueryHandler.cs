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

    public GetCreatedEventsByUserIdQueryHandler(IUnitOfWork unitOfWork, SignInManager<Domain.Aggregates.UserAggregate.User> signInManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<Pagination<EventDto>> Handle(GetCreatedEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "");

        List<Domain.Aggregates.EventAggregate.Event> events = await _unitOfWork.CachedEvents
            .FindByCondition(x => x.AuthorId == userId)
            .Include(x => x.EventCategories)
                .ThenInclude(x => x.Category)
            .ToListAsync(cancellationToken);

        List<EventDto> eventDtos = _mapper.Map<List<EventDto>>(events);

        return PagingHelper.Paginate<EventDto>(eventDtos, request.Filter);
    }
}
