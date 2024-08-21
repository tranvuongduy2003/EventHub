using EventHub.Application.Commands.Auth.ExternalLogin;
using EventHub.Application.Commands.Auth.ExternalLoginCallback;
using EventHub.Application.Commands.Auth.ForgotPassword;
using EventHub.Application.Commands.Auth.RefreshToken;
using EventHub.Application.Commands.Auth.ResetPassword;
using EventHub.Application.Commands.Auth.SignIn;
using EventHub.Application.Commands.Auth.SignOut;
using EventHub.Application.Commands.Auth.SignUp;
using EventHub.Application.Commands.Auth.ValidateUser;
using EventHub.Application.Queries.Auth.GetUserProfile;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.Auth;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.Exceptions;
using EventHub.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(SignInResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiValidationFilter]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto dto)
    {
        _logger.LogInformation("START: SignUp");
        try
        {
            var signUpResponse = await _mediator.Send(new SignUpCommand(dto));

            _logger.LogInformation("END: SignUp");

            return Ok(new ApiOkResponse(signUpResponse));
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

    [HttpPost("validate-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost("signin")]
    [ProducesResponseType(typeof(SignInResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignIn([FromBody] SignInDto dto)
    {
        _logger.LogInformation("START: SignIn");
        try
        {
            var signInResponse = await _mediator.Send(new SignInCommand(dto));

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
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost("signout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignOut()
    {
        _logger.LogInformation("START: SignOut");
        try
        {
            await _mediator.Send(new SignOutCommand());

            _logger.LogInformation("END: SignOut");

            Response.Cookies.Delete("AuthTokenHolder");

            return Ok(new ApiOkResponse());
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost("external-login")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
    {
        _logger.LogInformation("START: ExternalLogin");
        try
        {
            if (User.Identity != null) await _mediator.Send(new SignOutCommand());

            var externalLoginResponse = await _mediator.Send(new ExternalLoginCommand(provider, returnUrl));

            _logger.LogInformation("END: ExternalLogin");

            return Challenge(externalLoginResponse.Properties, externalLoginResponse.Provider);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("external-auth-callback")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExternalLoginCallback([FromQuery] string returnUrl)
    {
        _logger.LogInformation("START: ExternalLoginCallback");
        try
        {
            var signInResponse = await _mediator.Send(new ExternalLoginCallbackCommand(returnUrl));

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

            return Redirect(returnUrl);
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
    
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(SignInResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [TokenRequirementFilter]
    [ApiValidationFilter]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        _logger.LogInformation("START: RefreshToken");
        try
        {
            var accessToken = Request
                .Headers[HeaderNames.Authorization]
                .ToString()
                .Replace("Bearer ", "");

            var refreshTokenResponse = await _mediator.Send(new RefreshTokenCommand(dto.RefreshToken, accessToken));

            _logger.LogInformation("END: RefreshToken");

            return Ok(new ApiOkResponse(refreshTokenResponse));
        }
        catch (UnauthorizedException e)
        {
            return Unauthorized(new ApiUnauthorizedResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        catch (Exception)
        {
            throw;
        }
    }
    
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        catch (Exception)
        {
            throw;
        }
    }
    
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.VIEW)]
    public async Task<IActionResult> GetUserProfile()
    {
        _logger.LogInformation("START: GetUserProfile");
        try
        {
            var accessToken = Request
                .Headers[HeaderNames.Authorization]
                .ToString()
                .Replace("Bearer ", "");

            var user = await _mediator.Send(new GetUserProfileQuery(accessToken));

            _logger.LogInformation("END: GetUserProfile");

            return Ok(new ApiOkResponse(user));
        }
        catch (UnauthorizedException e)
        {
            return Unauthorized(new ApiUnauthorizedResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }
}