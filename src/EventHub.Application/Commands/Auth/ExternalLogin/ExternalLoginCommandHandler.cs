using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Auth.ExternalLogin;

public class ExternalLoginCommandHandler : ICommandHandler<ExternalLoginCommand, ExternalLoginDto>
{
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger _logger;

    public ExternalLoginCommandHandler(SignInManager<User> signInManager, ILogger<ExternalLoginCommandHandler> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }
    
    public async Task<ExternalLoginDto> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: ExternalLoginCommandHandler");
        
        var redirectUrl =
            $"https://eventhubsolutionbackendserverplan.azurewebsites.net/api/auth/external-auth-callback?returnUrl={request.ReturnUrl}";
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(request.Provider, redirectUrl);
        properties.AllowRefresh = true;
        
        _logger.LogInformation("END: ExternalLoginCommandHandler");

        var externalLoginResponse = new ExternalLoginDto
        {
            Properties = properties,
            Provider = request.Provider
        };

        return externalLoginResponse;
    }
}