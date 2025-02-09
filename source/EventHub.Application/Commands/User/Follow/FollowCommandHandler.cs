using EventHub.Application.Hubs;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Enums.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.Follow;

public class FollowCommandHandler : ICommandHandler<FollowCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<NotificationHub> _hubContext;

    public FollowCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }

    public async Task Handle(FollowCommand request, CancellationToken cancellationToken)
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

        var notification = new NotificationDto
        {
            Title = "You have been followed",
            Message = $"You have been followed by {follower!.FullName}",
            Type = ENotificationType.FOLLOWING,
        };
        await _hubContext.Clients.User(followedUser.Id.ToString()).SendAsync("SendNotificationToUser", followedUser.Id, notification, cancellationToken);
    }
}
