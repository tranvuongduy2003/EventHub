using EventHub.Abstractions;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace EventHub.Infrastructure.FilterAttributes;

/// <summary>
/// Represents a filter used for authorization based on claim requirements.
/// </summary>
public class ClaimRequirementFilter : IAuthorizationFilter
{
    private readonly ECommandCode _eCommandCode;
    private readonly EFunctionCode _eFunctionCode;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimRequirementFilter"/> class.
    /// </summary>
    /// <param name="eFunctionCode">
    /// The function code representing the specific functionality or access level required for authorization.
    /// </param>
    /// <param name="eCommandCode">
    /// The command code representing the specific command or operation that needs to be authorized.
    /// </param>
    /// <param name="tokenService">
    /// An instance of <see cref="ITokenService"/> used to handle token-based operations, such as validation and claims retrieval.
    /// </param>
    public ClaimRequirementFilter(EFunctionCode eFunctionCode, ECommandCode eCommandCode, ITokenService tokenService)
    {
        _eFunctionCode = eFunctionCode;
        _eCommandCode = eCommandCode;
        _tokenService = tokenService;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var requestAuthorization = context.HttpContext.Request.Headers[HeaderNames.Authorization];
        var responseAuthorization = context.HttpContext.Response.Headers[HeaderNames.Authorization];

        var accessToken = !string.IsNullOrEmpty(requestAuthorization)
            ? requestAuthorization.ToString().Replace("Bearer ", "")
            : responseAuthorization.ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(accessToken))
        {
            context.Result = new ForbidResult();
            return;
        }

        var principal = _tokenService.GetPrincipalFromToken(accessToken);

        var permissionsClaim = principal.Claims
            .SingleOrDefault(c => c.Type == SystemConstants.Claims.Permissions);
        if (permissionsClaim != null)
        {
            var permissions = JsonConvert.DeserializeObject<List<string>>(permissionsClaim.Value);
            if (!permissions.Contains(_eFunctionCode + "_" + _eCommandCode))
            {
                context.Result = new ForbidResult();
            }

            var userId = principal.Claims
                .SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            context.HttpContext.Items["AuthorId"] = userId;
        }
        else
        {
            context.Result = new ForbidResult();
        }
    }
}