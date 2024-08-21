using EventHub.Application.Queries.Command.GetCommandsInFunction;
using EventHub.Application.Queries.Command.GetCommandsNotInFunction;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.Command;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/commands")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CommandsController> _logger;

    public CommandsController(ILogger<CommandsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpGet("get-in-function/{functionId}")]
    [ProducesResponseType(typeof(List<CommandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.SYSTEM_COMMAND, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCommandsInFunction(string functionId)
    {
        _logger.LogInformation("START: GetCommandsInFunction");
        try
        {
            var commands = await _mediator.Send(new GetCommandsInFunctionQuery(functionId));

            _logger.LogInformation("END: GetCommandsInFunction");

            return Ok(new ApiOkResponse(commands));
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    [HttpGet("get-not-in-function/{functionId}")]
    [ProducesResponseType(typeof(List<CommandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.SYSTEM_COMMAND, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCommandsNotInFunction(string functionId)
    {
        _logger.LogInformation("START: GetCommandsNotInFunction");
        try
        {
            var commands = await _mediator.Send(new GetCommandsNotInFunctionQuery(functionId));

            _logger.LogInformation("END: GetCommandsNotInFunction");

            return Ok(new ApiOkResponse(commands));
        }
        catch (Exception)
        {
            throw;
        }
    }
}