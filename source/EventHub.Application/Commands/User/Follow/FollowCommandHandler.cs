using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Enums.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.Follow;

public class FollowCommandHandler : ICommandHandler<FollowCommand, Guid>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public FollowCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, IUnitOfWork unitOfWork, INotificationService notificationService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<Guid> Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        string followerId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)
        ?.Value ?? "";

        Domain.Aggregates.UserAggregate.User follower = (await _userManager.FindByIdAsync(followerId))!;

        Domain.Aggregates.UserAggregate.User followedUser = await _userManager.FindByIdAsync(request.FollowedUserId.ToString());
        if (followedUser == null)
        {
            throw new NotFoundException($"Followed user does not exist!");
        }

        bool isFollowed = await _unitOfWork.UserFollowers
        .ExistAsync(x =>
                x.FollowerId == Guid.Parse(followerId) &&
                x.FollowedId == request.FollowedUserId);
        if (isFollowed)
        {
            throw new BadRequestException("User has been followed before");
        }

        var userFollower = new UserFollower
        {
            FollowerId = Guid.Parse(followerId),
            FollowedId = request.FollowedUserId,
        };
        await _unitOfWork.UserFollowers.CreateAsync(userFollower);
        await _unitOfWork.CommitAsync();

        follower.NumberOfFolloweds += 1;
        followedUser.NumberOfFollowers += 1;

        await _userManager.UpdateAsync(follower);
        await _userManager.UpdateAsync(followedUser);

        var notification = new SendNotificationDto
        {
            Type = ENotificationType.FOLLOWING,
            UserFollowerId = userFollower.Id,
        };
        await _notificationService.SendNotification(followedUser.Id.ToString(), notification);

        return userFollower.Id;
    }
}
