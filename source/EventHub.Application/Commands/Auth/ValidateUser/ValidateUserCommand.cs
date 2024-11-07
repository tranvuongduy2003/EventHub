using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.User;

namespace EventHub.Application.Commands.Auth.ValidateUser;

/// <summary>
/// Represents a command to validate a user's information.
/// </summary>
/// <remarks>
/// This command is used to validate user details such as email, phone number, and full name.
/// </remarks>
public class ValidateUserCommand : ICommand<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidateUserCommand"/> class.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing the user details to be validated.
    /// </param>
    public ValidateUserCommand(ValidateUserDto dto)
        => (Email, PhoneNumber, FullName) = (dto.Email, dto.PhoneNumber, dto.FullName);

    /// <summary>
    /// Gets or sets the email address of the user to be validated.
    /// </summary>
    /// <value>
    /// A string representing the user's email address.
    /// </value>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user to be validated.
    /// </summary>
    /// <value>
    /// A string representing the user's phone number.
    /// </value>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the full name of the user to be validated.
    /// </summary>
    /// <value>
    /// A string representing the user's full name.
    /// </value>
    public string FullName { get; set; }
}