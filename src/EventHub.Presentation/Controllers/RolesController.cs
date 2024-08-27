using EventHub.Application.Commands.Permission.AddFunctionToRole;
using EventHub.Application.Commands.Permission.RemoveFunctionFromRole;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.Command;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.Exceptions;
using EventHub.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpPost("{roleId:guid}/add-function/{functionId}")]
    [ProducesResponseType(typeof(List<CommandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
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
        catch (Exception)
        {
            throw;
        }
    }
    
    [HttpPost("{roleId:guid}/remove-function/{functionId}")]
    [ProducesResponseType(typeof(List<CommandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_PERMISSION, ECommandCode.VIEW)]
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
        catch (Exception)
        {
            throw;
        }
    }
}