using System.Security.Claims;
using System.Text;
using EventHub.Application.Abstractions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Domain.Shared.Constants;
using EventHub.Domain.Shared.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

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
    private readonly ILogger _logger;
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
        JwtOptions jwtOptions, ILogger logger)
    {
        _jwtOptions = jwtOptions;
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        var tokenHandler = new JsonWebTokenHandler();

        byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        IList<string> roles = await _userManager.GetRolesAsync(user);
        IQueryable<string> query = from p in _context.Permissions
                                   join c in _context.Commands
                                       on p.CommandId equals c.Id
                                   join f in _context.Functions
                                       on p.FunctionId equals f.Id
                                   join r in _roleManager.Roles on p.RoleId equals r.Id
                                   where roles.Contains(r.Name ?? "")
                                   select f.Id + "_" + c.Id;
        List<string> permissions = await query.Distinct().ToListAsync();

        var claimList = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
            new(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
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

        string token = tokenHandler.CreateToken(tokenDescriptor);
        return token;
    }

    public string GenerateRefreshToken()
    {
        var tokenHandler = new JsonWebTokenHandler();

        byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        string token = tokenHandler.CreateToken(tokenDescriptor);
        return token;
    }

    public async Task<ClaimsIdentity?> GetPrincipalFromToken(string token)
    {
        byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidAudience = _jwtOptions.Audience,
            ValidIssuer = _jwtOptions.Issuer,
        };

        var tokenHandler = new JsonWebTokenHandler();

        TokenValidationResult principal = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
        if (principal.Exception is not null)
        {
            _logger.Error(principal.Exception.Message);
            return null;
        }

        return principal.ClaimsIdentity;
    }

    public bool ValidateTokenExpired(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var tokenHandler = new JsonWebTokenHandler();

        SecurityToken jwtToken = tokenHandler.ReadToken(token);

        if (jwtToken is null)
        {
            return false;
        }

        return jwtToken.ValidTo > DateTime.UtcNow;
    }
}
