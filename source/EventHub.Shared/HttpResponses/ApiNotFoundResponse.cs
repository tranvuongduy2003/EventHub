using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventHub.Shared.HttpResponses;

public class ApiNotFoundResponse : ApiResponse
{
    public ApiNotFoundResponse(ModelStateDictionary modelState)
        : base(404)
    {
        if (modelState.IsValid)
        {
            throw new ArgumentException("ModelState must be invalid", nameof(modelState));
        }

        Errors = Enumerable.ToArray<string>(modelState.SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage));
    }

    public ApiNotFoundResponse(IdentityResult identityResult)
        : base(404)
    {
        Errors = Enumerable.ToArray<string>(identityResult.Errors
                .Select(x => x.Code + " - " + x.Description));
    }

    public ApiNotFoundResponse(string message)
        : base(404, message)
    {
    }
}
