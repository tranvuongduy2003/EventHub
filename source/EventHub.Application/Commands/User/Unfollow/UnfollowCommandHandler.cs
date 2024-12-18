using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
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
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(userId);

        user?.UnfollowUser(user.Id, request.FollowedUserId);
        await _unitOfWork.CommitAsync();
    }
}
