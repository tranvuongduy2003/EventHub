using EventHub.Shared.DTOs.Auth;
using MediatR;

namespace EventHub.Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommand : IRequest<SignInResponseDto>
{
    public RefreshTokenCommand(string refreshToken, string accessToken)
        => (RefreshToken, AccessToken) = (refreshToken, accessToken);
    
    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }
}