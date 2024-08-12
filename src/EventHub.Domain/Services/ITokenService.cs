using System.Security.Claims;
using EventHub.Domain.AggregateModels.UserAggregate;

namespace EventHub.Domain.Services;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(User user);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetPrincipalFromToken(string token);

    bool ValidateTokenExpired(string token);
}