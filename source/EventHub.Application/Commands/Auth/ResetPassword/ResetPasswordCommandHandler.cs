using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.ResetPassword;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, bool>
{
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public ResetPasswordCommandHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new NotFoundException("User does not exist");
        }

        IdentityResult result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
        {
            throw new BadRequestException(result);
        }
        
        return true;
    }
}
