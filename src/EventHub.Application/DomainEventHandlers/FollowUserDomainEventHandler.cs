using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class FollowUserDomainEventHandler : IDomainEventHandler<FollowUserDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public FollowUserDomainEventHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Handle(FollowUserDomainEvent notification, CancellationToken cancellationToken)
    {
        var follower = await _userManager.FindByIdAsync(notification.FollowerId.ToString());
        if (follower == null)
            throw new NotFoundException($"Follower does not exist!");

        var followedUser = await _userManager.FindByIdAsync(notification.FollowedUserId.ToString());
        if (followedUser == null)
            throw new NotFoundException($"Followed user does not exist!");

        var isFollowed = await _unitOfWork.UserFollowers
            .ExistAsync(x =>
                x.FollowerId.Equals(notification.FollowerId) &&
                x.FollowedId.Equals(notification.FollowedUserId));
        if (isFollowed)
            throw new BadRequestException("User has been followed before");

        var userFollower = new UserFollower
        {
            FollowerId = notification.FollowerId,
            FollowedId = notification.FollowedUserId,
        };
        await _unitOfWork.UserFollowers.CreateAsync(userFollower);
        await _unitOfWork.CommitAsync();

        follower.NumberOfFolloweds += 1;
        followedUser.NumberOfFollowers += 1;

        await _userManager.UpdateAsync(follower);
        await _userManager.UpdateAsync(followedUser);
    }
}