using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Enums.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Notification.GetNotificationsStatistic;

public class GetNotificationsStatisticQueryHandler : IQueryHandler<GetNotificationsStatisticQuery, NotificationStatisticDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public GetNotificationsStatisticQueryHandler(IUnitOfWork unitOfWork,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
    }

    public Task<NotificationStatisticDto> Handle(GetNotificationsStatisticQuery request,
        CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        IQueryable<Domain.Aggregates.NotificationAggregate.Notification> query = _unitOfWork.Notifications
            .FindByCondition(x => x.TargetUserId == Guid.Parse(userId));

        int totalUnseenFollowing = query.Count(x => !x.IsSeen && x.Type == ENotificationType.FOLLOWING);
        int totalUnseenOrdering = query.Count(x => !x.IsSeen && x.Type == ENotificationType.ORDERING);
        int totalUnseenInvitation = query.Count(x => !x.IsSeen && x.Type == ENotificationType.INVITING);

        return Task.FromResult(new NotificationStatisticDto
        {
            TotalUnseenFollowing = totalUnseenFollowing,
            TotalUnseenInvitation = totalUnseenInvitation,
            TotalUnseenOrdering = totalUnseenOrdering
        });
    }
}
