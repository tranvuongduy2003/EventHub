using EventHub.Shared.HttpResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventHub.Infrastructure.FilterAttributes;

/// <summary>
/// An action filter attribute that validates the model state before the action method executes.
/// </summary>
/// <remarks>
/// This filter checks the `ModelState` of the action context to ensure that the model binding and validation are successful.
/// If the model state is invalid, it sets the result to a `BadRequestObjectResult` with details about the validation errors.
/// This helps in providing consistent validation responses for API requests.
/// </remarks>
public class ApiValidationFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Executes the validation logic before the action method executes.
    /// </summary>
    /// <param name="context">The context for the action that is executing.</param>
    /// <remarks>
    /// If the model state is not valid, this method will set the `Result` property of the `ActionExecutingContext`
    /// to a `BadRequestObjectResult` that includes details about the validation errors encapsulated in an
    /// `ApiBadRequestResponse` object. This prevents the action method from executing if there are validation errors.
    /// </remarks>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            // Set the result to a BadRequestObjectResult with the validation errors
            context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(context.ModelState));
        }

        // Call the base method to ensure any additional logic in the base class is executed
        base.OnActionExecuting(context);
    }
}
