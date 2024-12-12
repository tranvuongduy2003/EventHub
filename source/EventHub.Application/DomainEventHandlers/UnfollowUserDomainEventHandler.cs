using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class UnfollowUserDomainEventHandler : IDomainEventHandler<UnfollowUserDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public UnfollowUserDomainEventHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Handle(UnfollowUserDomainEvent notification, CancellationToken cancellationToken)
    {
        User follower = await _userManager.FindByIdAsync(notification.FollowerId.ToString());
        if (follower == null)
        {
            throw new NotFoundException($"Follower does not exist!");
        }

        User followedUser = await _userManager.FindByIdAsync(notification.FollowedUserId.ToString());
        if (followedUser == null)
        {
            throw new NotFoundException($"Followed user does not exist!");
        }

        UserFollower userFollower = await _unitOfWork.UserFollowers
            .FindByCondition(x =>
                x.FollowerId == notification.FollowerId &&
                x.FollowedId == notification.FollowedUserId)
            .FirstOrDefaultAsync(cancellationToken);
        if (userFollower == null)
        {
            throw new BadRequestException("User has not been followed before");
        }

        await _unitOfWork.UserFollowers.Delete(userFollower);
        await _unitOfWork.CommitAsync();

        follower.NumberOfFolloweds -= 1;
        followedUser.NumberOfFollowers -= 1;

        await _userManager.UpdateAsync(follower);
        await _userManager.UpdateAsync(followedUser);
    }
}
