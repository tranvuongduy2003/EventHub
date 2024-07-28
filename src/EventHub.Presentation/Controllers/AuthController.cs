using EventHub.Domain.DTOs.Auth;
using EventHub.Domain.DTOs.User;
using EventHub.Domain.Exceptions;
using EventHub.Domain.HttpResponses;
using EventHub.Domain.Models.Auth;
using EventHub.Domain.Models.User;
using EventHub.Domain.Usecases;
using EventHub.Infrastructor.FilterAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EventHub.Presentation.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthUsecase _authUsecase;
    private readonly ILogger _logger;

    public AuthController(ILogger logger, IAuthUsecase authUsecase)
    {
        _logger = logger;
        _authUsecase = authUsecase;
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(SignInResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiValidationFilter]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto dto)
    {
        _logger.LogInformation("START: AuthController - SignUp");
        try
        {
            var signUpResponse = await _authUsecase.SignUpAsync(dto);

            _logger.LogInformation("END: AuthController - SignUp");

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
        _logger.LogInformation("START: AuthController - ValidateUser");
        try
        {
            await _authUsecase.ValidateUserAsync(dto);

            _logger.LogInformation("END: AuthController - ValidateUser");

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
    [ProducesResponseType(typeof(SignInResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignIn([FromBody] SignInDto dto)
    {
        _logger.LogInformation("START: AuthController - SignIn");
        try
        {
            var signInResponse = await _authUsecase.SignInAsync(dto);

            _logger.LogInformation("END: AuthController - SignIn");

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
        _logger.LogInformation("START: AuthController - SignOut");
        try
        {
            await _authUsecase.SignOutAsync();

            _logger.LogInformation("END: AuthController - SignOut");

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
        _logger.LogInformation("START: AuthController - ExternalLogin");
        try
        {
            if (User.Identity != null) await _authUsecase.SignOutAsync();

            var externalLoginResponse = await _authUsecase.ExternalLoginAsync(provider, returnUrl);

            _logger.LogInformation("END: AuthController - ExternalLogin");

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
        _logger.LogInformation("START: AuthController - ExternalLoginCallback");
        try
        {
            var signInResponse = await _authUsecase.ExternalLoginCallbackAsync(returnUrl);

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

            _logger.LogInformation("END: AuthController - ExternalLoginCallback");

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

    [Authorize]
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(SignInResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiValidationFilter]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        _logger.LogInformation("START: AuthController - RefreshToken");
        try
        {
            var accessToken = Request
                .Headers[HeaderNames.Authorization]
                .ToString()
                .Replace("Bearer ", "");

            var refreshTokenResponse = await _authUsecase.RefreshTokenAsync(dto, accessToken);

            _logger.LogInformation("END: AuthController - RefreshToken");

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

    [Authorize]
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiValidationFilter]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        _logger.LogInformation("START: AuthController - ForgotPassword");
        try
        {
            await _authUsecase.ForgotPasswordAsync(dto);

            _logger.LogInformation("END: AuthController - ForgotPassword");

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

    [Authorize]
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiValidationFilter]
    public async Task<IActionResult> ResetPassword([FromBody] ResetUserPasswordDto dto)
    {
        _logger.LogInformation("START: AuthController - ResetPassword");
        try
        {
            await _authUsecase.ResetPasswordAsync(dto);

            _logger.LogInformation("END: AuthController - ResetPassword");

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

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserProfile()
    {
        _logger.LogInformation("START: AuthController - GetUserProfile");
        try
        {
            var accessToken = Request
                .Headers[HeaderNames.Authorization]
                .ToString()
                .Replace("Bearer ", "");

            var userProfile = await _authUsecase.GetUserProfileAsync(accessToken);

            _logger.LogInformation("END: AuthController - GetUserProfile");

            return Ok(new ApiOkResponse(userProfile));
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