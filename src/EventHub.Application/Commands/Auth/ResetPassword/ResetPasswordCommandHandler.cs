using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Auth.ResetPassword;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, bool>
{
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager, ILogger<ResetPasswordCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: ResetPasswordCommandHandler");
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) throw new NotFoundException("User does not exist");

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded) throw new BadRequestException(result);
        
        _logger.LogInformation("BEGIN: ResetPasswordCommandHandler");

        return true;
    }
}