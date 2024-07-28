using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventHub.Domain.HttpResponses;

public class ApiNotFoundResponse : ApiResponse
{
    public ApiNotFoundResponse(ModelStateDictionary modelState)
        : base(404)
    {
        if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = modelState.SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();
    }

    public ApiNotFoundResponse(IdentityResult identityResult)
        : base(404)
    {
        Errors = identityResult.Errors
            .Select(x => x.Code + " - " + x.Description).ToArray();
    }

    public ApiNotFoundResponse(string message)
        : base(404, message)
    {
    }
}