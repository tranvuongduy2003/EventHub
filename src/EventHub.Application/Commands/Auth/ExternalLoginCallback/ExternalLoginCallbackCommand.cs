using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.DTOs.Auth;

namespace EventHub.Application.Commands.Auth.ExternalLoginCallback;

/// <summary>
/// Represents a command to handle the callback process after an external login attempt.
/// </summary>
public class ExternalLoginCallbackCommand : ICommand<SignInResponseDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalLoginCallbackCommand"/> class.
    /// </summary>
    /// <param name="returnUrl">
    /// The URL to which the user will be redirected after the external login callback process is complete.
    /// </param>
    public ExternalLoginCallbackCommand(string returnUrl)
        => ReturnUrl = returnUrl;
    
    /// <summary>
    /// Gets or sets the URL to which the user will be redirected after the external login callback process is complete.
    /// </summary>
    /// <value>
    /// A string representing the return URL for redirection after the external login callback.
    /// </value>
    public string ReturnUrl { get; set; }
}