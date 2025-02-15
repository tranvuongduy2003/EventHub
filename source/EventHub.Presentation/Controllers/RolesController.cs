using EventHub.Application.Commands.Permission.AddFunctionToRole;
using EventHub.Application.Commands.Permission.RemoveFunctionFromRole;
using EventHub.Application.Queries.Role.GetPaginatedRoles;
using EventHub.Application.SeedWork.Attributes;
using EventHub.Application.SeedWork.DTOs.Command;
using EventHub.Application.SeedWork.DTOs.Role;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using EventHub.Domain.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/roles")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RolesController> _logger;

    public RolesController(ILogger<RolesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerResponse(200, "Successfully retrieved the list of roles", typeof(Pagination<RoleDto>))]
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_ROLE, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedRoles([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedRoles");

        Pagination<RoleDto> roles = await _mediator.Send(new GetPaginatedRolesQuery(filter));

        _logger.LogInformation("END: GetPaginatedRoles");

        return Ok(new ApiOkResponse(roles));
    }

    [HttpPost("{roleId:guid}/add-function/{functionId}")]
    [SwaggerOperation(
        Summary = "Add a function to a role",
        Description = "Adds a specific function to the role identified by the role ID."
    )]
    [SwaggerResponse(200, "Function added to role successfully", typeof(List<CommandDto>))]
    [SwaggerResponse(400, "Bad Request - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Role or function with the specified IDs not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_PERMISSION, ECommandCode.VIEW)]
    public async Task<IActionResult> PostAddFunctionToRole(Guid roleId, string functionId)
    {
        _logger.LogInformation("START: PostAddFunctionToRole");
        try
        {
            await _mediator.Send(new AddFunctionToRoleCommand(functionId, roleId));

            _logger.LogInformation("END: PostAddFunctionToRole");

            return Ok(new ApiOkResponse());
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
    }

    [HttpPost("{roleId:guid}/remove-function/{functionId}")]
    [SwaggerOperation(
        Summary = "Remove a function from a role",
        Description = "Removes a specific function from the role identified by the role ID."
    )]
    [SwaggerResponse(200, "Function removed from role successfully", typeof(List<CommandDto>))]
    [SwaggerResponse(400, "Bad Request - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Role or function with the specified IDs not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_PERMISSION, ECommandCode.VIEW)]
    public async Task<IActionResult> PostRemoveFunctionFromRole(Guid roleId, string functionId)
    {
        _logger.LogInformation("START: PostRemoveFunctionFromRole");
        try
        {
            await _mediator.Send(new RemoveFunctionFromRoleCommand(functionId, roleId));

            _logger.LogInformation("END: PostRemoveFunctionFromRole");

            return Ok(new ApiOkResponse());
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }
}
