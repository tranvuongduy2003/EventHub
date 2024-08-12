using EventHub.Shared.DTOs.Auth;
using MediatR;

namespace EventHub.Application.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommand : IRequest<bool>
{
    public ForgotPasswordCommand(ForgotPasswordDto dto)
        => Email = dto.Email;
    
    public string Email { get; init; }
}