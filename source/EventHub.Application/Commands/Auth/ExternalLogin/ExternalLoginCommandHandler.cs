using EventHub.Application.DTOs.Auth;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.ExternalLogin;

public class ExternalLoginCommandHandler : ICommandHandler<ExternalLoginCommand, ExternalLoginDto>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public ExternalLoginCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<ExternalLoginDto> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            string redirectUrl =
                $"https://eventhubsolutionbackendserverplan.azurewebsites.net/api/auth/external-auth-callback?returnUrl={request.ReturnUrl}";
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(request.Provider, redirectUrl);
            properties.AllowRefresh = true;

            var externalLoginResponse = new ExternalLoginDto
            {
                Properties = properties,
                Provider = request.Provider
            };

            return externalLoginResponse;
        }, cancellationToken);
    }
}
