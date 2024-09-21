using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Auth;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Enums.User;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.Auth.SignUp;

/// <summary>
/// Represents a command to sign up a new user.
/// </summary>
/// <remarks>
/// This command is used to create a new user account with the provided details.
/// </remarks>
public class SignUpCommand : ICommand<SignInResponseDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SignUpCommand"/> class.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing the details required to create a new user account.
    /// </param>
    public SignUpCommand(CreateUserDto dto)
        => (Email, PhoneNumber, Dob, FullName, Password, UserName, Gender, Bio, Avatar)
            = (dto.Email, dto.PhoneNumber, dto.Dob, dto.FullName, dto.Password, dto.UserName, dto.Gender, dto.Bio,
                dto.Avatar);

    /// <summary>
    /// Gets or sets the email address for the new user account.
    /// </summary>
    /// <value>
    /// A string representing the email address of the user.
    /// </value>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number for the new user account.
    /// </summary>
    /// <value>
    /// A string representing the phone number of the user.
    /// </value>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the date of birth for the new user account.
    /// </summary>
    /// <value>
    /// A nullable <see cref="DateTime"/> representing the date of birth of the user.
    /// </value>
    public DateTime? Dob { get; set; }

    /// <summary>
    /// Gets or sets the full name of the new user.
    /// </summary>
    /// <value>
    /// A string representing the full name of the user.
    /// </value>
    public string FullName { get; set; }

    /// <summary>
    /// Gets or sets the password for the new user account.
    /// </summary>
    /// <value>
    /// A string representing the password for the user account.
    /// </value>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the username for the new user account.
    /// </summary>
    /// <value>
    /// A string representing the username of the user. This property is optional and can be an empty string.
    /// </value>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gender of the new user.
    /// </summary>
    /// <value>
    /// An optional <see cref="EGender"/> enumeration value representing the gender of the user.
    /// </value>
    public EGender? Gender { get; set; }

    /// <summary>
    /// Gets or sets the biography or personal description for the new user.
    /// </summary>
    /// <value>
    /// An optional string representing the user's bio or personal description.
    /// </value>
    public string? Bio { get; set; }

    /// <summary>
    /// Gets or sets the avatar image for the new user account.
    /// </summary>
    /// <value>
    /// An optional <see cref="IFormFile"/> representing the avatar image file for the user.
    /// </value>
    public IFormFile? Avatar { get; set; }
}