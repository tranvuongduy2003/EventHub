using EventHub.Application.Commands.User.ChangePassword;
using EventHub.Application.Commands.User.CreateUser;
using EventHub.Application.Commands.User.UpdateUser;
using EventHub.Application.Queries.User.GetPaginatedUsers;
using EventHub.Application.Queries.User.GetUserById;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.Exceptions;
using EventHub.Shared.HttpResponses;
using EventHub.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new user",
        Description = "Creates a new user based on the provided details. The request must include multipart form data."
    )]
    [SwaggerResponse(201, "User created successfully", typeof(UserDto))]
    [SwaggerResponse(400, "BadRequest - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [Consumes("multipart/form-data")]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.CREATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PostCreateUser([FromForm] CreateUserDto request)
    {
        _logger.LogInformation("START: PostCreateUser");
        try
        {
            var user = await _mediator.Send(new CreateUserCommand(request));

            _logger.LogInformation("END: PostCreateUser");

            return Ok(new ApiCreatedResponse(user));
        }
        catch (BadRequestException e)
        {
            return NotFound(new ApiBadRequestResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Retrieve a list of users",
        Description = "Fetches a paginated list of users based on the provided filter parameters."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of users", typeof(Pagination<UserDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedUsers([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedUsers");
        try
        {
            var users = await _mediator.Send(new GetPaginatedUsersQuery(filter));

            _logger.LogInformation("END: GetPaginatedUsers");

            return Ok(new ApiOkResponse(users));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("{userId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a user by its ID",
        Description = "Fetches the details of a specific user based on the provided user ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the user", typeof(UserDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.VIEW)]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        _logger.LogInformation("START: GetUserById");
        try
        {
            var user = await _mediator.Send(new GetUserByIdQuery(userId));

            _logger.LogInformation("END: GetUserById");

            return Ok(new ApiOkResponse(user));
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

    [HttpPut("{userId:guid}")]
    [SwaggerOperation(
        Summary = "Update an existing user",
        Description = "Updates the details of an existing user based on the provided user ID and update information."
    )]
    [SwaggerResponse(200, "User updated successfully")]
    [SwaggerResponse(400, "BadRequest - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [Consumes("multipart/form-data")]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PutUpdateUser(Guid userId, [FromForm] UpdateUserDto request)
    {
        _logger.LogInformation("START: PutUpdateUser");
        try
        {
            await _mediator.Send(new UpdateUserCommand(userId, request));

            _logger.LogInformation("END: PutUpdateUser");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (BadRequestException e)
        {
            return NotFound(new ApiBadRequestResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    [HttpPatch("{userId:guid}/change-password")]
    [SwaggerOperation(
        Summary = "Change a user's password",
        Description = "Changes the password of an existing user based on the provided user ID and new password information."
    )]
    [SwaggerResponse(200, "Password changed successfully")]
    [SwaggerResponse(400, "BadRequest - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PatchChangeUserPassword(Guid userId, [FromBody] UpdateUserPasswordDto request)
    {
        _logger.LogInformation("START: PatchChangeUserPassword");
        try
        {
            await _mediator.Send(new ChangePasswordCommand(userId, request));

            _logger.LogInformation("END: PatchChangeUserPassword");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (BadRequestException e)
        {
            return NotFound(new ApiBadRequestResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }
}