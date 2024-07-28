using EventHub.Domain.Enums.Command;
using EventHub.Domain.Enums.Function;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Infrastructor.FilterAttributes;

public class ClaimRequirementAttribute : TypeFilterAttribute
{
    public ClaimRequirementAttribute(EFunctionCode eFunctionId, ECommandCode eCommandId)
        : base(typeof(ClaimRequirementFilter))
    {
        Arguments = new object[] { eFunctionId, eCommandId };
    }
}