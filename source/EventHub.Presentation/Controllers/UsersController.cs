using EventHub.Application.Commands.User.ChangePassword;
using EventHub.Application.Commands.User.CreateUser;
using EventHub.Application.Commands.User.Follow;
using EventHub.Application.Commands.User.InviteUsers;
using EventHub.Application.Commands.User.Unfollow;
using EventHub.Application.Commands.User.UpdateUser;
using EventHub.Application.Queries.User.GetPaginatedFollowers;
using EventHub.Application.Queries.User.GetPaginatedFollowingUsers;
using EventHub.Application.Queries.User.GetPaginatedUsers;
using EventHub.Application.Queries.User.GetUserById;
using EventHub.Application.Queries.User.GetUserProfile;
using EventHub.Application.SeedWork.Attributes;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using EventHub.Domain.Shared.SeedWork;
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
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.CREATE)]
    public async Task<IActionResult> PostCreateUser([FromForm] CreateUserDto request)
    {
        _logger.LogInformation("START: PostCreateUser");
        try
        {
            UserDto user = await _mediator.Send(new CreateUserCommand(request));

            _logger.LogInformation("END: PostCreateUser");

            return Ok(new ApiCreatedResponse(user));
        }
        catch (BadRequestException e)
        {
            return NotFound(new ApiBadRequestResponse(e.Message));
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
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedUsers([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedUsers");

        Pagination<UserDto> users = await _mediator.Send(new GetPaginatedUsersQuery(filter));

        _logger.LogInformation("END: GetPaginatedUsers");

        return Ok(new ApiOkResponse(users));
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
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.VIEW)]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        _logger.LogInformation("START: GetUserById");
        try
        {
            UserDto user = await _mediator.Send(new GetUserByIdQuery(userId));

            _logger.LogInformation("END: GetUserById");

            return Ok(new ApiOkResponse(user));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));

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
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.UPDATE)]
    public async Task<IActionResult> PutUpdateUser(Guid userId, [FromForm] UpdateUserDto request)
    {
        _logger.LogInformation("START: PutUpdateUser");
        try
        {
            UserDto user = await _mediator.Send(new UpdateUserCommand(userId, request));

            _logger.LogInformation("END: PutUpdateUser");

            return Ok(new ApiOkResponse(user));
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

    [HttpPatch("{userId:guid}/change-password")]
    [SwaggerOperation(
        Summary = "Change a user's password",
        Description =
            "Changes the password of an existing user based on the provided user ID and new password information."
    )]
    [SwaggerResponse(200, "Password changed successfully")]
    [SwaggerResponse(400, "BadRequest - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.UPDATE)]
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
    }

    [HttpGet("{userId:guid}/followers")]
    [SwaggerOperation(
        Summary = "Retrieve a list of followers of a user by its ID",
        Description = "Fetches a paginated list of followers based on the provided user ID and filter parameters."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of followers", typeof(Pagination<UserDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedFollowers(Guid userId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedFollowers");

        Pagination<UserDto> users = await _mediator.Send(new GetPaginatedFollowersQuery(userId, filter));

        _logger.LogInformation("END: GetPaginatedFollowers");

        return Ok(new ApiOkResponse(users));

    }


    [HttpGet("{userId:guid}/following-users")]
    [SwaggerOperation(
        Summary = "Retrieve a list of following users of a user by its ID",
        Description = "Fetches a paginated list of following users based on the provided user ID and filter parameters."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of following users", typeof(Pagination<UserDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedFollowingUsers(Guid userId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedFollowingUsers");

        Pagination<UserDto> users = await _mediator.Send(new GetPaginatedFollowingUsersQuery(userId, filter));

        _logger.LogInformation("END: GetPaginatedFollowingUsers");

        return Ok(new ApiOkResponse(users));

    }

    [HttpPatch("follow/{followedUserId:guid}")]
    [SwaggerOperation(
        Summary = "Follow a user",
        Description = "Allows the authenticated user to follow another user by specifying the followed user's ID."
    )]
    [SwaggerResponse(200, "User followed successfully")]
    [SwaggerResponse(400, "BadRequest - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.UPDATE)]
    public async Task<IActionResult> PatchFollowUser(Guid followedUserId)
    {
        _logger.LogInformation("START: PatchFollowUser");
        try
        {
            await _mediator.Send(new FollowCommand(followedUserId));

            _logger.LogInformation("END: PatchFollowUser");

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
    }

    [HttpPatch("unfollow/{followedUserId:guid}")]
    [SwaggerOperation(
        Summary = "Unfollow a user",
        Description = "Allows the authenticated user to unfollow another user by specifying the followed user's ID."
    )]
    [SwaggerResponse(200, "User unfollowed successfully")]
    [SwaggerResponse(400, "BadRequest - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.UPDATE)]
    public async Task<IActionResult> PatchUnfollowUser(Guid followedUserId)
    {
        _logger.LogInformation("START: PatchUnfollowUser");
        try
        {
            await _mediator.Send(new UnfollowCommand(followedUserId));

            _logger.LogInformation("END: PatchUnfollowUser");

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
    }

    [HttpGet("profile")]
    [SwaggerOperation(
        Summary = "Retrieve user profile",
        Description = "Fetches the details of the currently authenticated user."
    )]
    [SwaggerResponse(200, "User profile retrieved successfully", typeof(UserDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.VIEW)]
    public async Task<IActionResult> GetUserProfile()
    {
        _logger.LogInformation("START: GetUserProfile");
        try
        {
            UserDto user = await _mediator.Send(new GetUserProfileQuery());

            _logger.LogInformation("END: GetUserProfile");

            return Ok(new ApiOkResponse(user));
        }
        catch (UnauthorizedException e)
        {
            return Unauthorized(new ApiUnauthorizedResponse(e.Message));
        }
    }

    [HttpPost("invitations")]
    [SwaggerResponse(200, "Users invited successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [Consumes("multipart/form-data")]
    [ClaimRequirement(EFunctionCode.ADMINISTRATION_USER, ECommandCode.UPDATE)]
    public async Task<IActionResult> PostInviteUsers([FromBody] InviteDto request)
    {
        _logger.LogInformation("START: PostInviteUsers");

        await _mediator.Send(new InviteUsersCommand(request.EventId, request.UserIds));

        _logger.LogInformation("END: PostInviteUsers");

        return Ok(new ApiOkResponse());
    }
}
