using EventHub.Application.Queries.Permission.GetFullPermissions;
using EventHub.Application.Queries.Permission.GetPermissionsCategorizedByRoles;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.Command;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/permissions")]
[ApiController]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PermissionsController> _logger;

    public PermissionsController(ILogger<PermissionsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<CommandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
    public async Task<IActionResult> GetFullPermissions(string functionId)
    {
        _logger.LogInformation("START: GetFullPermissions");
        try
        {
            var permissions = await _mediator.Send(new GetFullPermissionsQuery());

            _logger.LogInformation("END: GetFullPermissions");

            return Ok(new ApiOkResponse(permissions));
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    [HttpGet("roles")]
    [ProducesResponseType(typeof(List<CommandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPermissionsCategorizedByRoles()
    {
        _logger.LogInformation("START: GetPermissionsCategorizedByRoles");
        try
        {
            var permissions = await _mediator.Send(new GetPermissionsCategorizedByRolesQuery());

            _logger.LogInformation("END: GetPermissionsCategorizedByRoles");

            return Ok(new ApiOkResponse(permissions));
        }
        catch (Exception)
        {
            throw;
        }
    }
}