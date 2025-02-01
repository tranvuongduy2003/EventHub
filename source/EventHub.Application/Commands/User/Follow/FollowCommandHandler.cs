using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.Follow;

public class FollowCommandHandler : ICommandHandler<FollowCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public FollowCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, IUnitOfWork unitOfWork)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
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
    }
}
