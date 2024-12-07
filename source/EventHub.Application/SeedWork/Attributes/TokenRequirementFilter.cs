using System.Security.Claims;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Shared.HttpResponses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace EventHub.Application.SeedWork.Attributes;

/// <summary>
/// Represents a filter used for authorization based on token requirements.
/// </summary>
public class TokenRequirementFilter : IAuthorizationFilter
{
    private readonly ITokenService _tokenService;
    private readonly SignInManager<User> _signInManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenRequirementFilter"/> class.
    /// </summary>
    /// <param name="tokenService">
    /// An instance of <see cref="ITokenService"/> used to handle token-based operations, such as validation and retrieval of claims.
    /// </param>
    public TokenRequirementFilter(ITokenService tokenService, SignInManager<User> signInManager)
    {
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string requestAuthorization = context.HttpContext.Request.Headers[HeaderNames.Authorization];
        string responseAuthorization = context.HttpContext.Response.Headers[HeaderNames.Authorization];

        string accessToken = !string.IsNullOrEmpty(requestAuthorization)
            ? requestAuthorization.ToString().Replace("Bearer ", "")
            : responseAuthorization?.ToString().Replace("Bearer ", "") ?? "";

        if (!_tokenService.ValidateTokenExpired(accessToken))
        {
            context.Result = new UnauthorizedObjectResult(new ApiUnauthorizedResponse("invalid_token"));
        }

        ClaimsIdentity principal = _tokenService.GetPrincipalFromToken(accessToken).GetAwaiter().GetResult();

        if (principal == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        _signInManager.Context.User.AddIdentity(principal);
    }
}
