using EventHub.Application.DTOs.Auth;
using EventHub.Domain.SeedWork.Command;

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
    public SignUpCommand(SignUpDto dto)
        => (Email, PhoneNumber, FullName, Password, UserName)
            = (dto.Email, dto.PhoneNumber, dto.FullName, dto.Password, dto.UserName);

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
}