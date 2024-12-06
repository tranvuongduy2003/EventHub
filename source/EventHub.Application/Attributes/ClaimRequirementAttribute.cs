using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Application.Attributes;

/// <summary>
/// An attribute used to enforce claim-based authorization requirements for actions or controllers.
/// </summary>
/// <remarks>
/// This attribute is a custom implementation of the `TypeFilterAttribute` that applies a specific authorization filter
/// based on claim requirements. It is used to specify that a user must have specific function and command claims
/// to access the decorated action or controller.
/// </remarks>
public sealed class ClaimRequirementAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimRequirementAttribute"/> class.
    /// </summary>
    /// <param name="eFunctionId">The function code that specifies the required function claim.</param>
    /// <param name="eCommandId">The command code that specifies the required command claim.</param>
    /// <remarks>
    /// This constructor initializes the attribute with the specified function and command codes.
    /// The `Arguments` property of the base `TypeFilterAttribute` class is set with these values to be passed
    /// to the `ClaimRequirementFilter` when it is instantiated.
    /// </remarks>
    public ClaimRequirementAttribute(EFunctionCode eFunctionId, ECommandCode eCommandId)
        : base(typeof(ClaimRequirementFilter))
    {
        // Set the arguments for the ClaimRequirementFilter
        Arguments = new object[] { eFunctionId, eCommandId };
    }
}
