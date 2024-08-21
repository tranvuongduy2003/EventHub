using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Services;
using EventHub.Persistence.Data;
using EventHub.Shared.Settings;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace EventHub.Infrastructure.Services;

/// <summary>
/// Provides functionality for generating, validating, and managing authentication tokens.
/// </summary>
/// <remarks>
/// This class implements the <see cref="ITokenService"/> interface and typically handles operations such as
/// creating new tokens, validating existing tokens, and retrieving claims from tokens. It may use libraries
/// or frameworks for token generation and validation, such as JWT (JSON Web Tokens).
/// </remarks>
public class TokenService : ITokenService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtOptions _jwtOptions;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenService"/> class.
    /// </summary>
    /// <param name="context">
    /// An instance of <see cref="ApplicationDbContext"/> used to interact with the application database.
    /// </param>
    /// <param name="userManager">
    /// An instance of <see cref="UserManager{User}"/> used for managing user-related operations.
    /// </param>
    /// <param name="roleManager">
    /// An instance of <see cref="RoleManager{Role}"/> used for managing role-related operations.
    /// </param>
    /// <param name="jwtOptions">
    /// An instance of <see cref="JwtOptions"/> containing configuration settings for JWT tokens.
    /// </param>
    public TokenService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager,
        JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var roles = await _userManager.GetRolesAsync(user);
        var query = from p in _context.Permissions
            join c in _context.Commands
                on p.CommandId equals c.Id
            join f in _context.Functions
                on p.FunctionId equals f.Id
            join r in _roleManager.Roles on p.RoleId equals r.Id
            where roles.Contains(r.Name)
            select f.Id + "_" + c.Id;
        var permissions = await query.Distinct().ToListAsync();

        var claimList = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
            new(JwtRegisteredClaimNames.Jti, user.Id),
            new(ClaimTypes.Role, string.Join(";", roles)),
            new(SystemConstants.Claims.Permissions, JsonConvert.SerializeObject(permissions))
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Subject = new ClaimsIdentity(claimList),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        );

        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    public bool ValidateTokenExpired(string token)
    {
        if (token is null || token == "") return false;

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtToken = tokenHandler.ReadToken(token);

        if (jwtToken is null) return false;

        return jwtToken.ValidTo > DateTime.UtcNow;
    }
}