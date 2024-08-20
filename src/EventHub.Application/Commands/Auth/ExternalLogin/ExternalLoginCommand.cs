using EventHub.Shared.DTOs.Auth;
using MediatR;

namespace EventHub.Application.Commands.Auth.ExternalLogin;

public class ExternalLoginCommand : IRequest<ExternalLoginDto>
{
    public ExternalLoginCommand(string provider, string returnUrl)
        => (Provider, ReturnUrl) = (provider, returnUrl);
    
    public string Provider { get; set; }

    public string ReturnUrl { get; set; }
}