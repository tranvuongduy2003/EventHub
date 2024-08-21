using EventHub.Application.Commands.Function.CreateFunction;
using EventHub.Application.Commands.Function.DeleteFunction;
using EventHub.Application.Commands.Function.UpdateFunction;
using EventHub.Application.Queries.Function.GetFunctionById;
using EventHub.Application.Queries.Function.GetFuntions;
using EventHub.Domain.Events;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.Function;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.Exceptions;
using EventHub.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/functions")]
[ApiController]
public class FunctionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<FunctionsController> _logger;

    public FunctionsController(ILogger<FunctionsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(FunctionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.CREATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PostFunction([FromBody] CreateFunctionDto request)
    {
        _logger.LogInformation("START: PostFunction");
        try
        {
            var function = await _mediator.Send(new CreateFunctionCommand(request));

            _logger.LogInformation("END: PostFunction");

            return Ok(new ApiCreatedResponse(function));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<FunctionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.VIEW)]
    public async Task<IActionResult> GetFunctions()
    {
        _logger.LogInformation("START: GetFunctions");
        try
        {
            var functions = await _mediator.Send(new GetFunctionsQuery());

            _logger.LogInformation("END: GetFunctions");

            return Ok(new ApiOkResponse(functions));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("{functionId}")]
    [ProducesResponseType(typeof(FunctionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.VIEW)]
    public async Task<IActionResult> GetFunctionById(string functionId)
    {
        _logger.LogInformation("START: GetFunctionById");
        try
        {
            var function = await _mediator.Send(new GetFunctionByIdQuery(functionId));

            _logger.LogInformation("END: GetFunctionById");

            return Ok(new ApiOkResponse(function));
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

    [HttpPut("{functionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PutFunction(string functionId, [FromBody] UpdateFunctionDto request)
    {
        _logger.LogInformation("START: PutFunction");
        try
        {
            await _mediator.Send(new UpdateFunctionCommand(functionId, request));

            _logger.LogInformation("END: PutFunction");

            return Ok(new ApiOkResponse(true));
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

    [HttpDelete("{functionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.DELETE)]
    public async Task<IActionResult> DeleteFunction(string functionId)
    {
        _logger.LogInformation("START: DeleteFunction");
        try
        {
            await _mediator.Send(new DeleteFunctionCommand(functionId));

            _logger.LogInformation("END: DeleteFunction");

            return Ok(new ApiOkResponse(true));
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

    [HttpPost("{functionId}/enable-command/{commandId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PostEnableCommandInFunction(string functionId, string commandId)
    {
        _logger.LogInformation("START: PostEnableCommandInFunction");
        try
        {
            await _mediator.Send(new EnableCommandInFunctionDomainEvent(functionId, commandId));

            _logger.LogInformation("END: PostEnableCommandInFunction");

            return Ok(new ApiOkResponse(true));
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

    [HttpPost("{functionId}/disable-command/{commandId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PostDisableCommandInFunction(string functionId, string commandId)
    {
        _logger.LogInformation("START: PostDisableCommandInFunction");
        try
        {
            await _mediator.Send(new DisableCommandInFunctionDomainEvent(functionId, commandId));

            _logger.LogInformation("END: PostDisableCommandInFunction");

            return Ok(new ApiOkResponse(true));
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