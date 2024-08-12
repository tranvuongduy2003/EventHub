using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) throw new NotFoundException("User does not exist");

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded) throw new BadRequestException(result);

        return true;
    }
}