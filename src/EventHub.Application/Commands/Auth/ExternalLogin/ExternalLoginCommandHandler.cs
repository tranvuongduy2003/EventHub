using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Shared.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.ExternalLogin;

public class ExternalLoginCommandHandler : IRequestHandler<ExternalLoginCommand, ExternalLoginModel>
{
    private readonly SignInManager<User> _signInManager;

    public ExternalLoginCommandHandler(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }
    
    public async Task<ExternalLoginModel> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        var redirectUrl =
            $"https://eventhubsolutionbackendserverplan.azurewebsites.net/api/auth/external-auth-callback?returnUrl={request.ReturnUrl}";
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(request.Provider, redirectUrl);
        properties.AllowRefresh = true;

        var externalLoginResponse = new ExternalLoginModel
        {
            Properties = properties,
            Provider = request.Provider
        };

        return externalLoginResponse;
    }
}