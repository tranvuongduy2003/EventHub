using EventHub.Shared.DTOs.Auth;
using MediatR;

namespace EventHub.Application.Commands.Auth.ExternalLoginCallback;

public class ExternalLoginCallbackCommand : IRequest<SignInResponseDto>
{
    public ExternalLoginCallbackCommand(string returnUrl)
        => ReturnUrl = returnUrl;
    
    public string ReturnUrl { get; set; }
}