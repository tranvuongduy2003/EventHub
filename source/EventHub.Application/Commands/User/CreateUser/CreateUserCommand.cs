using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Enums.User;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.User.CreateUser;

/// <summary>
/// Represents a command to create a new user.
/// </summary>
/// <remarks>
/// This command is used to create a new user with the specified details.
/// </remarks>
public class CreateUserCommand : ICommand<UserDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommand"/> class.
    /// </summary>
    /// <param name="request">
    /// The data transfer object containing the details for the new user.
    /// </param>
    public CreateUserCommand(CreateUserDto request)
        => (Email, PhoneNumber, Dob, FullName, Password, UserName, Gender, Bio, Avatar) =
            (request.Email, request.PhoneNumber, request.Dob, request.FullName, request.Password, request.UserName,
                request.Gender, request.Bio, request.Avatar);

    /// <summary>
    /// Gets or sets the email address of the new user.
    /// </summary>
    /// <value>
    /// A string representing the email address of the user.
    /// </value>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the new user.
    /// </summary>
    /// <value>
    /// A string representing the phone number of the user.
    /// </value>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the date of birth of the new user.
    /// </summary>
    /// <value>
    /// A nullable <see cref="DateTime"/> representing the user's date of birth.
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
    /// Gets or sets the password for the new user.
    /// </summary>
    /// <value>
    /// A string representing the password of the user.
    /// </value>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the username of the new user.
    /// </summary>
    /// <value>
    /// A nullable string representing the username of the user.
    /// </value>
    public string? UserName { get; set; }

    /// <summary>
    /// Gets or sets the gender of the new user.
    /// </summary>
    /// <value>
    /// A nullable <see cref="EGender"/> enum representing the gender of the user.
    /// </value>
    public EGender? Gender { get; set; }

    /// <summary>
    /// Gets or sets the bio of the new user.
    /// </summary>
    /// <value>
    /// A nullable string representing the bio of the user.
    /// </value>
    public string? Bio { get; set; }

    /// <summary>
    /// Gets or sets the avatar image file for the new user.
    /// </summary>
    /// <value>
    /// A nullable <see cref="IFormFile"/> representing the avatar image of the user.
    /// </value>
    public IFormFile? Avatar { get; set; }
}
