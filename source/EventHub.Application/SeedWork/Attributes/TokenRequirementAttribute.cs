using Microsoft.AspNetCore.Mvc;

namespace EventHub.Application.SeedWork.Attributes;

/// <summary>
/// Represents an attribute that applies a <see cref="TokenRequirementFilter"/> to an action or controller.
/// </summary>
public sealed class TokenRequirementAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenRequirementAttribute"/> class.
    /// </summary>
    public TokenRequirementAttribute()
        : base(typeof(TokenRequirementFilter))
    {
        Arguments = Array.Empty<object>();
    }
}
