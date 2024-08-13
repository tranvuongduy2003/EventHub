using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Infrastructure.FilterAttributes;

public class ClaimRequirementAttribute : TypeFilterAttribute
{
    public ClaimRequirementAttribute(EFunctionCode eFunctionId, ECommandCode eCommandId)
        : base(typeof(ClaimRequirementFilter))
    {
        Arguments = new object[] { eFunctionId, eCommandId };
    }
}