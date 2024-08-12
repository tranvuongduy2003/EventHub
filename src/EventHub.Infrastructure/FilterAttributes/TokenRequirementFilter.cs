using EventHub.Domain.Services;
using EventHub.Shared.HttpResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace EventHub.Infrastructor.FilterAttributes;

public class TokenRequirementFilter : IAuthorizationFilter
{
    private readonly ITokenService _tokenService;

    public TokenRequirementFilter(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var requestAuthorization = context.HttpContext.Request.Headers[HeaderNames.Authorization];
        var responseAuthorization = context.HttpContext.Response.Headers[HeaderNames.Authorization];

        var accessToken = !string.IsNullOrEmpty(requestAuthorization)
            ? requestAuthorization.ToString().Replace("Bearer ", "")
            : responseAuthorization.ToString().Replace("Bearer ", "");

        if (_tokenService.ValidateTokenExpired(accessToken))
            context.Result = new UnauthorizedObjectResult(new ApiUnauthorizedResponse("invalid_token"));
    }
}