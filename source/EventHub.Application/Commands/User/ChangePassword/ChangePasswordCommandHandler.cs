using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.User.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(request.UserId.ToString());

        user?.ChangeUserPassword(request.UserId, request.OldPassword, request.NewPassword);
        await _unitOfWork.CommitAsync();
    }
}
