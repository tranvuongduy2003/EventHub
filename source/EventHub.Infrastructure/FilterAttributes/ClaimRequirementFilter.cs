using System.Security.Claims;
using EventHub.Abstractions.Services;
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
        string requestAuthorization = context.HttpContext.Request.Headers[HeaderNames.Authorization];
        string responseAuthorization = context.HttpContext.Response.Headers[HeaderNames.Authorization];

        string accessToken = !string.IsNullOrEmpty(requestAuthorization)
            ? requestAuthorization.ToString().Replace("Bearer ", "")
            : responseAuthorization?.ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(accessToken))
        {
            context.Result = new ForbidResult();
            return;
        }

        ClaimsIdentity principal = _tokenService.GetPrincipalFromToken(accessToken).GetAwaiter().GetResult();

        Claim permissionsClaim = principal?.Claims
            .SingleOrDefault(c => c.Type == SystemConstants.Claims.Permissions);
        if (permissionsClaim != null)
        {
            List<string> permissions = JsonConvert.DeserializeObject<List<string>>(permissionsClaim.Value);
            if (permissions != null && !permissions.Contains(_eFunctionCode + "_" + _eCommandCode))
            {
                context.Result = new ForbidResult();
            }

            string userId = principal!.Claims
                .SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? "";
            context.HttpContext.Items["AuthorId"] = userId;
        }
        else
        {
            context.Result = new ForbidResult();
        }
    }
}
