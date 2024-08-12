using Microsoft.AspNetCore.Mvc;

namespace EventHub.Infrastructor.FilterAttributes;

public class TokenRequirementAttribute : TypeFilterAttribute
{
    public TokenRequirementAttribute()
        : base(typeof(TokenRequirementFilter))
    {
        Arguments = new object[] { };
    }
}