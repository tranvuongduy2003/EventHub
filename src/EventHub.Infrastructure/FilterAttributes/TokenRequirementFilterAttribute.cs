using Microsoft.AspNetCore.Mvc;

namespace EventHub.Infrastructure.FilterAttributes;

public class TokenRequirementFilterAttribute : TypeFilterAttribute
{
    public TokenRequirementFilterAttribute()
        : base(typeof(TokenRequirementFilter))
    {
        Arguments = new object[] { };
    }
}