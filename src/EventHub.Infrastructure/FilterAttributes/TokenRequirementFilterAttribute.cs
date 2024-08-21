using Microsoft.AspNetCore.Mvc;

namespace EventHub.Infrastructure.FilterAttributes;

/// <summary>
/// Represents an attribute that applies a <see cref="TokenRequirementFilter"/> to an action or controller.
/// </summary>
public class TokenRequirementFilterAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenRequirementFilterAttribute"/> class.
    /// </summary>
    public TokenRequirementFilterAttribute()
        : base(typeof(TokenRequirementFilter))
    {
        Arguments = new object[] { };
    }
}