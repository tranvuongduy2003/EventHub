using EventHub.Shared.DTOs.User;
using MediatR;

namespace EventHub.Application.Commands.Auth.ResetPassword;

public class ResetPasswordCommand : IRequest<bool>
{
    public ResetPasswordCommand(ResetUserPasswordDto dto)
        => (NewPassword, Email, Token) = (dto.NewPassword, dto.Email, dto.Token);
    
    public string NewPassword { get; set; }

    public string Email { get; set; }

    public string Token { get; set; }
}