﻿using System.Security.Claims;
using EventHub.Application.Abstractions;
using EventHub.Domain.Shared.HttpResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;

namespace EventHub.Application.Attributes;

/// <summary>
/// Represents a filter used for authorization based on token requirements.
/// </summary>
public class TokenRequirementFilter : IAuthorizationFilter
{
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenRequirementFilter"/> class.
    /// </summary>
    /// <param name="tokenService">
    /// An instance of <see cref="ITokenService"/> used to handle token-based operations, such as validation and retrieval of claims.
    /// </param>
    public TokenRequirementFilter(ITokenService tokenService)
    {
        _tokenService = tokenService;
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
        string userId = principal?.Claims?
            .SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? "";
        context.HttpContext.Items["AuthorId"] = userId;
    }
}
