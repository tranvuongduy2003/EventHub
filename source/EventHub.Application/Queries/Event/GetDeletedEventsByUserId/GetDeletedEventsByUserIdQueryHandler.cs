using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Event.GetDeletedEventsByUserId;

public class GetDeletedEventsByUserIdQueryHandler : IQueryHandler<GetDeletedEventsByUserIdQuery, Pagination<EventDto>>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;

    public GetDeletedEventsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public Task<Pagination<EventDto>> Handle(GetDeletedEventsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()
            ?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "");

        Pagination<Domain.Aggregates.EventAggregate.Event> paginatedEvents = _unitOfWork.Events
            .GetPaginatedDeletedEvents(request.Filter, query => query
                .Where(x => x.AuthorId == userId)
                .Include(x => x.EventCategories)
                .ThenInclude(x => x.Category));

        Pagination<EventDto> paginatedEventDtos = _mapper.Map<Pagination<EventDto>>(paginatedEvents);

        return Task.FromResult(paginatedEventDtos);
    }
}
