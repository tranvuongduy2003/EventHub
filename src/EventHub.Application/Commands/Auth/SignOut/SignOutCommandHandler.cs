using EventHub.Application.Commands.Auth.SignIn;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Auth.SignOut;

public class SignOutCommandHandler : ICommandHandler<SignOutCommand>
{
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<SignOutCommandHandler> _logger;

    public SignOutCommandHandler(SignInManager<User> signInManager, ILogger<SignOutCommandHandler> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }
    
    public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: SignOutCommandHandler");
        
        await _signInManager.SignOutAsync();
        
        _logger.LogInformation("END: SignOutCommandHandler");
    }
}