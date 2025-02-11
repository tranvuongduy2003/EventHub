using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Enums.Notification;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Notification.GetPaginatedNotifications;

public class GetPaginatedNotificationsQueryHandler : IQueryHandler<GetPaginatedNotificationsQuery, Pagination<NotificationDto, NotificationMetadata>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public GetPaginatedNotificationsQueryHandler(IUnitOfWork unitOfWork,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public Task<Pagination<NotificationDto, NotificationMetadata>> Handle(GetPaginatedNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        int totalUnseenFollowing = 0;
        int totalUnseenOrdering = 0;
        int totalUnseenInvitation = 0;

        Pagination<Domain.Aggregates.NotificationAggregate.Notification> paginatedNotifications = _unitOfWork.Notifications
                .PaginatedFindByCondition(x => x.TargetUserId == Guid.Parse(userId), request.Filter,
                    query =>
                    {
                        totalUnseenFollowing = query.Count(x => !x.IsSeen && x.Type == ENotificationType.FOLLOWING);
                        totalUnseenOrdering = query.Count(x => !x.IsSeen && x.Type == ENotificationType.ORDERING);
                        totalUnseenInvitation = query.Count(x => !x.IsSeen && x.Type == ENotificationType.INVITING);

                        if (request.Filter.Type != null)
                        {
                            query = request.Filter.Type switch
                            {
                                ENotificationType.FOLLOWING => query.Where(x => x.Type == ENotificationType.FOLLOWING),
                                ENotificationType.INVITING => query.Where(x => x.Type == ENotificationType.INVITING),
                                ENotificationType.ORDERING => query.Where(x => x.Type == ENotificationType.ORDERING),
                                _ => query
                            };
                        }

                        return query
                            .Include(x => x.UserFollower)
                                .ThenInclude(x => x != null ? x.Follower : default)
                            .Include(x => x.Invitation)
                                .ThenInclude(x => x != null ? x.Event : default)
                            .Include(x => x.Invitation)
                                .ThenInclude(x => x != null ? x.Inviter : default)
                            .Include(x => x.Payment)
                                .ThenInclude(x => x != null ? x.Event : default)
                            .Include(x => x.Payment)
                                .ThenInclude(x => x != null ? x.Author : default);
                    }
                );

        Pagination<NotificationDto, NotificationMetadata> paginatedNotificationDtos = _mapper.Map<Pagination<NotificationDto, NotificationMetadata>>(paginatedNotifications);

        paginatedNotificationDtos.Metadata.TotalUnseenInvitation = totalUnseenInvitation;
        paginatedNotificationDtos.Metadata.TotalUnseenFollowing = totalUnseenFollowing;
        paginatedNotificationDtos.Metadata.TotalUnseenOrdering = totalUnseenOrdering;

        return Task.FromResult(paginatedNotificationDtos);
    }
}
