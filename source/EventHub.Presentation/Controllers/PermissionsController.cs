using EventHub.Application.Queries.Permission.GetFullPermissions;
using EventHub.Application.Queries.Permission.GetPermissionsByUser;
using EventHub.Application.Queries.Permission.GetPermissionsCategorizedByRoles;
using EventHub.Application.SeedWork.Attributes;
using EventHub.Application.SeedWork.DTOs.Permission;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(
        Summary = "Retrieve all permissions for a function",
        Description = "Fetches a list of all permissions associated with the specified function ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of permissions", typeof(List<FullPermissionDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
    public async Task<IActionResult> GetFullPermissions()
    {
        _logger.LogInformation("START: GetFullPermissions");

        List<FullPermissionDto> permissions = await _mediator.Send(new GetFullPermissionsQuery());

        _logger.LogInformation("END: GetFullPermissions");

        return Ok(new ApiOkResponse(permissions));

    }

    [HttpGet("roles")]
    [SwaggerOperation(
        Summary = "Retrieve permissions categorized by roles",
        Description = "Fetches a list of permissions categorized by roles."
    )]
    [SwaggerResponse(200, "Successfully retrieved the permissions categorized by roles", typeof(List<RolePermissionDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPermissionsCategorizedByRoles()
    {
        _logger.LogInformation("START: GetPermissionsCategorizedByRoles");

        List<RolePermissionDto> permissions = await _mediator.Send(new GetPermissionsCategorizedByRolesQuery());

        _logger.LogInformation("END: GetPermissionsCategorizedByRoles");

        return Ok(new ApiOkResponse(permissions));
    }

    [HttpGet("get-by-user/{userId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve permissions by user",
        Description = "Fetches a list of permissions by user id."
    )]
    [SwaggerResponse(200, "Successfully retrieved the permissions by user", typeof(List<RolePermissionDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "NotFound - User does not exist")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPermissionsByUser(Guid userId)
    {
        _logger.LogInformation("START: GetPermissionsByUser");

        List<RolePermissionDto> permissions = await _mediator.Send(new GetPermissionsByUserQuery(userId));

        _logger.LogInformation("END: GetPermissionsByUser");

        return Ok(new ApiOkResponse(permissions));
    }
}
