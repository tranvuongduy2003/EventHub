using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Auth;

namespace EventHub.Application.Commands.Auth.RefreshToken;

/// <summary>
/// Represents a command to refresh the authentication tokens for a user.
/// </summary>
public class RefreshTokenCommand : ICommand<SignInResponseDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenCommand"/> class.
    /// </summary>
    /// <param name="refreshToken">
    /// The refresh token used to obtain new authentication tokens.
    /// </param>
    /// <param name="accessToken">
    /// The current access token that is being refreshed.
    /// </param>
    public RefreshTokenCommand(string refreshToken, string accessToken)
        => (RefreshToken, AccessToken) = (refreshToken, accessToken);

    /// <summary>
    /// Gets or sets the refresh token used to obtain new authentication tokens.
    /// </summary>
    /// <value>
    /// A string representing the refresh token.
    /// </value>
    public string RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the current access token that is being refreshed.
    /// </summary>
    /// <value>
    /// A string representing the current access token.
    /// </value>
    public string AccessToken { get; set; }
}