using Microsoft.AspNetCore.Mvc;

namespace EventHub.Application.Attributes;

/// <summary>
/// Represents an attribute that applies a <see cref="TokenRequirementFilter"/> to an action or controller.
/// </summary>
public sealed class TokenRequirementFilterAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenRequirementFilterAttribute"/> class.
    /// </summary>
    public TokenRequirementFilterAttribute()
        : base(typeof(TokenRequirementFilter))
    {
        Arguments = Array.Empty<object>();
    }
}
