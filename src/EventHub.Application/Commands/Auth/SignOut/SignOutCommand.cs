using MediatR;

namespace EventHub.Application.Commands.Auth.SignOut;

/// <summary>
/// Represents a command to sign out the current user.
/// </summary>
/// <remarks>
/// This command does not require any parameters because it is intended to end the user’s session 
/// and invalidate any authentication tokens or cookies associated with the user.
/// </remarks>
public class SignOutCommand : IRequest
{
    // No additional properties or methods are required for this command.
}