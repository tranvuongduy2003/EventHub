using System.Security.Claims;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Shared.Constants;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace EventHub.Application.SeedWork.Attributes;

/// <summary>
/// Represents a filter used for authorization based on claim requirements.
/// </summary>
public class ClaimRequirementFilter : IAuthorizationFilter
{
    private readonly ECommandCode _eCommandCode;
    private readonly EFunctionCode _eFunctionCode;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<User> _signInManager;

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
    public ClaimRequirementFilter(EFunctionCode eFunctionCode, ECommandCode eCommandCode, ITokenService tokenService, SignInManager<User> signInManager)
    {
        _eFunctionCode = eFunctionCode;
        _eCommandCode = eCommandCode;
        _tokenService = tokenService;
        _signInManager = signInManager;
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
            context.Result = new UnauthorizedObjectResult(new ApiUnauthorizedResponse("invalid_token"));
            return;
        }

        ClaimsIdentity principal = _tokenService.GetPrincipalFromToken(accessToken).GetAwaiter().GetResult();

        if (principal is null)
        {
            context.Result = new UnauthorizedObjectResult(new ApiUnauthorizedResponse("invalid_token"));
            return;
        }

        Claim permissionsClaim = principal.Claims
            .SingleOrDefault(c => c.Type == SystemConstants.Claims.Permissions);
        if (permissionsClaim != null)
        {
            List<string> permissions = JsonConvert.DeserializeObject<List<string>>(permissionsClaim.Value);
            if (permissions != null && !permissions.Contains(_eFunctionCode + "_" + _eCommandCode))
            {
                context.Result = new ForbidResult();
            }

            _signInManager.Context.User.AddIdentity(principal);
        }
        else
        {
            context.Result = new ForbidResult();
        }
    }
}
