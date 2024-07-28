using System.Security.Claims;
using EventHub.Domain.Common.Entities;

namespace EventHub.Domain.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(User user);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetPrincipalFromToken(string token);

    bool ValidateTokenExpired(string token);
}