using System.Security.Claims;
using EventHub.Domain.AggregateModels.UserAggregate;

namespace EventHub.Abstractions.Services;

/// <summary>
/// Defines a contract for services that handle JWT token operations, including token generation, validation, and extraction of claims.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Asynchronously generates an access token for the specified user.
    /// </summary>
    /// <param name="user">
    /// The user for whom the access token is being generated. This user object typically contains user information 
    /// such as claims and roles.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a string representing the generated access token.
    /// </returns>
    Task<string> GenerateAccessTokenAsync(User user);

    /// <summary>
    /// Generates a refresh token.
    /// </summary>
    /// <returns>
    /// A string representing the generated refresh token. Refresh tokens are used to obtain new access tokens without requiring re-authentication.
    /// </returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Extracts the claims principal from the provided token.
    /// </summary>
    /// <param name="token">
    /// The JWT token from which to extract the claims principal. This token contains the claims associated with the user.
    /// </param>
    /// <returns>
    /// A <see cref="ClaimsPrincipal"/> object representing the claims extracted from the token, or null if the token is invalid or cannot be parsed.
    /// </returns>
    ClaimsPrincipal? GetPrincipalFromToken(string token);

    /// <summary>
    /// Validates whether the provided token has expired.
    /// </summary>
    /// <param name="token">
    /// The JWT token to validate. This token is checked for expiration to determine its validity.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the token has expired. Returns true if the token is expired, otherwise false.
    /// </returns>
    bool ValidateTokenExpired(string token);
}