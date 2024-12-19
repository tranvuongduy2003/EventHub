using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.User.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public ChangePasswordCommandHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            throw new NotFoundException("User does not exist!");
        }

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, request.OldPassword);

        if (!isPasswordValid)
        {
            throw new BadRequestException("Old password is wrong!");
        }

        IdentityResult result =
            await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            throw new BadRequestException(result);
        }
    }
}
