using EventHub.Application.Commands.Function.CreateFunction;
using EventHub.Application.Commands.Function.DeleteFunction;
using EventHub.Application.Commands.Function.UpdateFunction;
using EventHub.Application.Commands.Permission.DisableCommandInFunction;
using EventHub.Application.Commands.Permission.EnableCommandInFunction;
using EventHub.Application.Queries.Function.GetFunctionById;
using EventHub.Application.Queries.Function.GetFuntions;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.Function;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.Exceptions;
using EventHub.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(
        Summary = "Create a new function",
        Description = "Creates a new function based on the provided details."
    )]
    [SwaggerResponse(201, "Function created successfully", typeof(FunctionDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
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
    [SwaggerOperation(
        Summary = "Retrieve a list of functions",
        Description = "Fetches a list of all available functions."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of functions", typeof(List<FunctionDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]

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
    [SwaggerOperation(
        Summary = "Retrieve a function by its ID",
        Description = "Fetches the details of a specific function based on the provided function ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the function", typeof(FunctionDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Function with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
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
    [SwaggerOperation(
        Summary = "Update an existing function",
        Description = "Updates the details of an existing function based on the provided function ID and update information."
    )]
    [SwaggerResponse(200, "Function updated successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Function with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
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
    [SwaggerOperation(
        Summary = "Delete a function",
        Description = "Deletes the function with the specified ID."
    )]
    [SwaggerResponse(200, "Function deleted successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Function with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
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
    [SwaggerOperation(
        Summary = "Enable a command for a function",
        Description = "Enables a specific command for the function identified by the function ID."
    )]
    [SwaggerResponse(200, "Command enabled successfully for the function")]
    [SwaggerResponse(400, "Bad Request - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Function or command with the specified IDs not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PostEnableCommandInFunction(string functionId, string commandId)
    {
        _logger.LogInformation("START: PostEnableCommandInFunction");
        try
        {
            await _mediator.Send(new EnableCommandInFunctionCommand(functionId, commandId));

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
    [SwaggerOperation(
        Summary = "Disable a command for a function",
        Description = "Disables a specific command for the function identified by the function ID."
    )]
    [SwaggerResponse(200, "Command disabled successfully for the function")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Function or command with the specified IDs not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.SYSTEM_FUNCTION, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PostDisableCommandInFunction(string functionId, string commandId)
    {
        _logger.LogInformation("START: PostDisableCommandInFunction");
        try
        {
            await _mediator.Send(new DisableCommandInFunctionCommand(functionId, commandId));

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