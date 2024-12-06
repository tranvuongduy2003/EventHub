using EventHub.Application.DTOs.Auth;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Auth.ExternalLogin;

/// <summary>
/// Represents a command to initiate an external login process.
/// </summary>
public class ExternalLoginCommand : ICommand<ExternalLoginDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalLoginCommand"/> class.
    /// </summary>
    /// <param name="provider">
    /// The name of the external authentication provider (e.g., "Google", "Facebook").
    /// </param>
    /// <param name="returnUrl">
    /// The URL to which the user will be redirected after the external login process is complete.
    /// </param>
    public ExternalLoginCommand(string provider, Uri returnUrl)
        => (Provider, ReturnUrl) = (provider, returnUrl);

    /// <summary>
    /// Gets or sets the name of the external authentication provider.
    /// </summary>
    /// <value>
    /// A string representing the name of the external authentication provider (e.g., "Google", "Facebook").
    /// </value>
    public string Provider { get; set; }

    /// <summary>
    /// Gets or sets the URL to which the user will be redirected after the external login process is complete.
    /// </summary>
    /// <value>
    /// A string representing the return URL for redirection after the external login process.
    /// </value>
    public Uri ReturnUrl { get; set; }
}
