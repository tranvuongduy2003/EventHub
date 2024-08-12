using EventHub.Domain.Services;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace EventHub.Infrastructor.FilterAttributes;

public class ClaimRequirementFilter : IAuthorizationFilter
{
    private readonly ECommandCode _eCommandCode;
    private readonly EFunctionCode _eFunctionCode;
    private readonly ITokenService _tokenService;

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

        var permissionsClaim = principal.Claims.SingleOrDefault(c => c.Type == SystemConstants.Claims.Permissions);
        if (permissionsClaim != null)
        {
            var permissions = JsonConvert.DeserializeObject<List<string>>(permissionsClaim.Value);
            if (!permissions.Contains(_eFunctionCode + "_" + _eCommandCode)) context.Result = new ForbidResult();
        }
        else
        {
            context.Result = new ForbidResult();
        }
    }
}