using EventHub.Shared.Models.Auth;
using MediatR;

namespace EventHub.Application.Commands.Auth.ExternalLogin;

public class ExternalLoginCommand : IRequest<ExternalLoginModel>
{
    public ExternalLoginCommand(string provider, string returnUrl)
        => (Provider, ReturnUrl) = (provider, returnUrl);
    
    public string Provider { get; set; }

    public string ReturnUrl { get; set; }
}