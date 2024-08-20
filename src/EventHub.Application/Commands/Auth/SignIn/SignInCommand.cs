using EventHub.Shared.DTOs.Auth;
using MediatR;

namespace EventHub.Application.Commands.Auth.SignIn;

public class SignInCommand : IRequest<SignInResponseDto>
{
    public SignInCommand(SignInDto dto)
        => (Identity, Password) = (dto.Identity, dto.Password);
    
    public string Identity { get; set; }

    public string Password { get; set; }
}