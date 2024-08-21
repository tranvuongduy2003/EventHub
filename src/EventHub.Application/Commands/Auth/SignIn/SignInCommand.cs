using EventHub.Shared.DTOs.Auth;
using MediatR;

namespace EventHub.Application.Commands.Auth.SignIn;

/// <summary>
/// Represents a command to sign in a user using their credentials.
/// </summary>
public class SignInCommand : IRequest<SignInResponseDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SignInCommand"/> class.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing the user's sign-in credentials.
    /// </param>
    public SignInCommand(SignInDto dto)
        => (Identity, Password) = (dto.Identity, dto.Password);

    /// <summary>
    /// Gets or sets the identity used to sign in, such as a username, email, or phone number.
    /// </summary>
    /// <value>
    /// A string representing the user's identity for sign-in purposes.
    /// </value>
    public string Identity { get; set; }

    /// <summary>
    /// Gets or sets the password associated with the user's identity.
    /// </summary>
    /// <value>
    /// A string representing the user's password.
    /// </value>
    public string Password { get; set; }
}
