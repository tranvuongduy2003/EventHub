using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventHub.Domain.Shared.HttpResponses;

public class ApiBadRequestResponse : ApiResponse
{
    public ApiBadRequestResponse(ModelStateDictionary modelState)
        : base(400)
    {
        if (modelState.IsValid)
        {
            throw new ArgumentException("ModelState must be invalid", nameof(modelState));
        }

        Errors = Enumerable.ToArray<string>(modelState.SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage));
    }

    public ApiBadRequestResponse(IdentityResult identityResult)
        : base(400)
    {
        Errors = Enumerable.ToArray<string>(identityResult.Errors
                .Select(x => x.Code + " - " + x.Description));
    }

    public ApiBadRequestResponse(string message)
        : base(400, message)
    {
    }
}
