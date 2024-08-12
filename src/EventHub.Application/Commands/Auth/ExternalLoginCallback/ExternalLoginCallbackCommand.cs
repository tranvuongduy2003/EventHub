using EventHub.Shared.Models.Auth;
using MediatR;

namespace EventHub.Application.Commands.Auth.ExternalLoginCallback;

public class ExternalLoginCallbackCommand : IRequest<SignInResponseModel>
{
    public ExternalLoginCallbackCommand(string returnUrl)
        => ReturnUrl = returnUrl;
    
    public string ReturnUrl { get; set; }
}