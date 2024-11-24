using EventHub.Application.Queries.Command.GetCommandsInFunction;
using EventHub.Application.Queries.Command.GetCommandsNotInFunction;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.Command;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(
        Summary = "Retrieve commands associated with a specific function",
        Description = "Fetches a list of commands that are associated with the specified function ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of commands", typeof(List<CommandDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.SYSTEM_COMMAND, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCommandsInFunction(string functionId)
    {
        _logger.LogInformation("START: GetCommandsInFunction");

        List<CommandDto> commands = await _mediator.Send(new GetCommandsInFunctionQuery(functionId));

        _logger.LogInformation("END: GetCommandsInFunction");

        return Ok(new ApiOkResponse(commands));

    }

    [HttpGet("get-not-in-function/{functionId}")]
    [SwaggerOperation(
        Summary = "Retrieve commands not associated with a specific function",
        Description = "Fetches a list of commands that are not associated with the specified function ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of commands", typeof(List<CommandDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.VIEW)]
    [ClaimRequirement(EFunctionCode.SYSTEM_COMMAND, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCommandsNotInFunction(string functionId)
    {
        _logger.LogInformation("START: GetCommandsNotInFunction");

        List<CommandDto> commands = await _mediator.Send(new GetCommandsNotInFunctionQuery(functionId));

        _logger.LogInformation("END: GetCommandsNotInFunction");

        return Ok(new ApiOkResponse(commands));

    }
}
