using Microsoft.AspNetCore.Mvc;

namespace EventHub.Infrastructure.FilterAttributes;

public class TokenRequirementAttribute : TypeFilterAttribute
{
    public TokenRequirementAttribute()
        : base(typeof(TokenRequirementFilter))
    {
        Arguments = new object[] { };
    }
}