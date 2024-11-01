using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
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
        var follower = await _userManager.FindByIdAsync(notification.FollowerId.ToString());
        if (follower == null)
            throw new NotFoundException($"Follower does not exist!");

        var followedUser = await _userManager.FindByIdAsync(notification.FollowedUserId.ToString());
        if (followedUser == null)
            throw new NotFoundException($"Followed user does not exist!");

        var userFollower = await _unitOfWork.UserFollowers
            .FindByCondition(x =>
                x.FollowerId.Equals(notification.FollowerId) &&
                x.FollowedId.Equals(notification.FollowedUserId))
            .FirstOrDefaultAsync();
        if (userFollower == null)
            throw new BadRequestException("User has not been followed before");

        await _unitOfWork.UserFollowers.DeleteAsync(userFollower);
        await _unitOfWork.CommitAsync();

        follower.NumberOfFolloweds -= 1;
        followedUser.NumberOfFollowers -= 1;

        await _userManager.UpdateAsync(follower);
        await _userManager.UpdateAsync(followedUser);
    }
}