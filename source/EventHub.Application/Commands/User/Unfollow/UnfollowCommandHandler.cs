using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.Unfollow;

public class UnfollowCommandHandler : ICommandHandler<UnfollowCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public UnfollowCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, IUnitOfWork unitOfWork)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UnfollowCommand request, CancellationToken cancellationToken)
    {
        string followerId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        Domain.Aggregates.UserAggregate.User follower = await _userManager.FindByIdAsync(followerId);

        Domain.Aggregates.UserAggregate.User followedUser = await _userManager.FindByIdAsync(request.FollowedUserId.ToString());
        if (followedUser == null)
        {
            throw new NotFoundException($"Followed user does not exist!");
        }

        UserFollower userFollower = await _unitOfWork.UserFollowers
            .FindByCondition(x =>
                x.FollowerId == Guid.Parse(followerId) &&
                x.FollowedId == request.FollowedUserId)
            .FirstOrDefaultAsync(cancellationToken);
        if (userFollower == null)
        {
            throw new BadRequestException("User has not been followed before");
        }

        await _unitOfWork.UserFollowers.Delete(userFollower);
        await _unitOfWork.CommitAsync();

        follower!.NumberOfFolloweds -= 1;
        followedUser.NumberOfFollowers -= 1;

        await _userManager.UpdateAsync(follower);
        await _userManager.UpdateAsync(followedUser);
    }
}
