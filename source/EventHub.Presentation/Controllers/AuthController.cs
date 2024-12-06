using EventHub.Application.Attributes;
using EventHub.Application.Commands.Auth.ExternalLogin;
using EventHub.Application.Commands.Auth.ExternalLoginCallback;
using EventHub.Application.Commands.Auth.ForgotPassword;
using EventHub.Application.Commands.Auth.RefreshToken;
using EventHub.Application.Commands.Auth.ResetPassword;
using EventHub.Application.Commands.Auth.SignIn;
using EventHub.Application.Commands.Auth.SignOut;
using EventHub.Application.Commands.Auth.SignUp;
using EventHub.Application.Commands.Auth.ValidateUser;
using EventHub.Application.DTOs.Auth;
using EventHub.Application.DTOs.User;
using EventHub.Application.Exceptions;
using EventHub.Application.Queries.Auth.GetUserProfile;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;

    public AuthController(ILogger<AuthController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("signup")]
    [SwaggerOperation(
        Summary = "Sign up a new user",
        Description =
            "Registers a new user with the provided details. Returns a sign-in response upon successful registration."
    )]
    [SwaggerResponse(200, "User successfully registered", typeof(SignInResponseDto))]
    [SwaggerResponse(400, "Invalid user input")]
    [SwaggerResponse(500, "An error occurred while processing the request")]
    [ApiValidationFilter]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto dto)
    {
        _logger.LogInformation("START: SignUp");
        try
        {
            SignInResponseDto signUpResponse = await _mediator.Send(new SignUpCommand(dto));

            _logger.LogInformation("END: SignUp");

            return Ok(new ApiOkResponse(signUpResponse));
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
    }

    [HttpPost("validate-user")]
    [SwaggerOperation(
        Summary = "Validate user credentials",
        Description =
            "Validates the provided user credentials and returns an appropriate response based on the validation result."
    )]
    [SwaggerResponse(200, "User credentials are valid")]
    [SwaggerResponse(400, "Invalid user credentials or request data")]
    [SwaggerResponse(500, "An error occurred while processing the request")]
    [ApiValidationFilter]
    public async Task<IActionResult> ValidateUser([FromBody] ValidateUserDto dto)
    {
        _logger.LogInformation("START: ValidateUser");
        try
        {
            await _mediator.Send(new ValidateUserCommand(dto));

            _logger.LogInformation("END: ValidateUser");

            return Ok(new ApiOkResponse());
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
    }

    [HttpPost("signin")]
    [SwaggerOperation(
        Summary = "Sign in a user",
        Description =
            "Authenticates the user based on the provided credentials and returns a sign-in response if successful."
    )]
    [SwaggerResponse(200, "Successfully signed in", typeof(SignInResponseDto))]
    [SwaggerResponse(401, "Unauthorized - Invalid credentials")]
    [SwaggerResponse(404, "Not Found - User not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> SignIn([FromBody] SignInDto dto)
    {
        _logger.LogInformation("START: SignIn");
        try
        {
            SignInResponseDto signInResponse = await _mediator.Send(new SignInCommand(dto));

            _logger.LogInformation("END: SignIn");

            return Ok(new ApiOkResponse(signInResponse));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (UnauthorizedException e)
        {
            return Unauthorized(new ApiUnauthorizedResponse(e.Message));
        }
    }

    [HttpPost("signout")]
    [SwaggerOperation(
        Summary = "Sign out a user",
        Description = "Signs out the current user, invalidating their session or authentication token."
    )]
    [SwaggerResponse(200, "Successfully signed out")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> LogOut()
    {
        _logger.LogInformation("START: LogOut");

        await _mediator.Send(new SignOutCommand());

        _logger.LogInformation("END: LogOut");

        Response.Cookies.Delete("AuthTokenHolder");

        return Ok(new ApiOkResponse());

    }

    [HttpPost("external-login")]
    [SwaggerOperation(
        Summary = "Log in a user via an external provider",
        Description =
            "Authenticates the user using an external authentication provider (e.g., Google, Facebook) and returns a login response if successful."
    )]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> ExternalLogin(string provider, Uri returnUrl)
    {
        _logger.LogInformation("START: ExternalLogin");

        if (User.Identity != null)
        {
            await _mediator.Send(new SignOutCommand());
        }

        ExternalLoginDto externalLoginResponse = await _mediator.Send(new ExternalLoginCommand(provider, returnUrl));

        _logger.LogInformation("END: ExternalLogin");

        return Challenge(externalLoginResponse.Properties, externalLoginResponse.Provider);

    }

    [HttpGet("external-auth-callback")]
    [SwaggerOperation(
        Summary = "Callback endpoint for external authentication",
        Description =
            "Handles the callback from an external authentication provider and processes the authentication result."
    )]
    [SwaggerResponse(400, "Bad Request - Invalid or missing returnUrl parameter")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the authentication callback")]
    public async Task<IActionResult> ExternalLoginCallback([FromQuery] Uri returnUrl)
    {
        _logger.LogInformation("START: ExternalLoginCallback");
        try
        {
            SignInResponseDto signInResponse = await _mediator.Send(new ExternalLoginCallbackCommand(returnUrl));

            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(5)
            };

            Response.Cookies.Append(
                "AuthTokenHolder",
                JsonConvert.SerializeObject(signInResponse, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    Formatting = Formatting.Indented
                }), options);

            _logger.LogInformation("END: ExternalLoginCallback");

            return Redirect(returnUrl.ToString());
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
    }

    [HttpPost("refresh-token")]
    [SwaggerOperation(
        Summary = "Refresh user authentication token",
        Description = "Refreshes the user's authentication token based on the provided refresh token credentials."
    )]
    [SwaggerResponse(200, "Successfully refreshed the token", typeof(SignInResponseDto))]
    [SwaggerResponse(401, "Unauthorized - Invalid or expired refresh token")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [TokenRequirementFilter]
    [ApiValidationFilter]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        _logger.LogInformation("START: RefreshToken");
        try
        {
            string accessToken = Request
                .Headers[HeaderNames.Authorization]
                .ToString()
                .Replace("Bearer ", "");

            SignInResponseDto refreshTokenResponse = await _mediator.Send(new RefreshTokenCommand(dto.RefreshToken, accessToken));

            _logger.LogInformation("END: RefreshToken");

            return Ok(new ApiOkResponse(refreshTokenResponse));
        }
        catch (UnauthorizedException e)
        {
            return Unauthorized(new ApiUnauthorizedResponse(e.Message));
        }
    }

    [HttpPost("forgot-password")]
    [SwaggerOperation(
        Summary = "Initiate password recovery",
        Description = "Sends a password recovery email to the user based on the provided email address."
    )]
    [SwaggerResponse(200, "Password recovery email sent successfully")]
    [SwaggerResponse(401, "Unauthorized - Invalid or missing credentials")]
    [SwaggerResponse(404, "Not Found - User with the provided email address not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [TokenRequirementFilter]
    [ApiValidationFilter]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        _logger.LogInformation("START: ForgotPassword");
        try
        {
            await _mediator.Send(new ForgotPasswordCommand(dto));

            _logger.LogInformation("END: ForgotPassword");

            return Ok(new ApiOkResponse());
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpPost("reset-password")]
    [SwaggerOperation(
        Summary = "Reset user password",
        Description = "Resets the user's password based on the provided credentials and reset information."
    )]
    [SwaggerResponse(200, "Password reset successfully")]
    [SwaggerResponse(400, "Bad Request - Invalid input or parameters")]
    [SwaggerResponse(401, "Unauthorized - Invalid or expired credentials")]
    [SwaggerResponse(404, "Not Found - User or resource not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [TokenRequirementFilter]
    [ApiValidationFilter]
    public async Task<IActionResult> ResetPassword([FromBody] ResetUserPasswordDto dto)
    {
        _logger.LogInformation("START: ResetPassword");
        try
        {
            await _mediator.Send(new ResetPasswordCommand(dto));

            _logger.LogInformation("END: ResetPassword");

            return Ok(new ApiOkResponse());
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (BadRequestException e)
        {
            return BadRequest(new BadRequestException(e.Message));
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
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.VIEW)]
    public async Task<IActionResult> GetUserProfile()
    {
        _logger.LogInformation("START: GetUserProfile");
        try
        {
            if (!Guid.TryParse(HttpContext.Items["AuthorId"]!.ToString(), out Guid userId))
            {
                userId = Guid.NewGuid();
            }

            UserDto user = await _mediator.Send(new GetUserProfileQuery(userId));

            _logger.LogInformation("END: GetUserProfile");

            return Ok(new ApiOkResponse(user));
        }
        catch (UnauthorizedException e)
        {
            return Unauthorized(new ApiUnauthorizedResponse(e.Message));
        }
    }
}
