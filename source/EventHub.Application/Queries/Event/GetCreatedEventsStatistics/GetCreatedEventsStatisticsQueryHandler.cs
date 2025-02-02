using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Event.GetCreatedEventsStatistics;

public class GetCreatedEventsStatisticsQueryHandler : IQueryHandler<GetCreatedEventsStatisticsQuery, CreatedEventsStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public GetCreatedEventsStatisticsQueryHandler(IUnitOfWork unitOfWork,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
    }

    public async Task<CreatedEventsStatisticsDto> Handle(GetCreatedEventsStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()
            ?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "");

        int totalEvents = await _unitOfWork.Events
            .FindAll()
            .CountAsync(x => x.AuthorId == userId, cancellationToken);

        int totalPublicEvents = await _unitOfWork.Events
            .FindAll()
            .CountAsync(x => x.AuthorId == userId && !x.IsPrivate, cancellationToken);

        int totalPrivateEvents = await _unitOfWork.Events
            .FindAll()
            .CountAsync(x => x.AuthorId == userId && x.IsPrivate, cancellationToken);

        return new CreatedEventsStatisticsDto
        {
            TotalEvents = totalEvents,
            TotalPublicEvents = totalPublicEvents,
            TotalPrivateEvents = totalPrivateEvents
        };
    }
}
