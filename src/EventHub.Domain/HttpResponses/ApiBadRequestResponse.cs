using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventHub.Domain.HttpResponses;

public class ApiBadRequestResponse : ApiResponse
{
    public ApiBadRequestResponse(ModelStateDictionary modelState)
        : base(400)
    {
        if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = modelState.SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();
    }

    public ApiBadRequestResponse(IdentityResult identityResult)
        : base(400)
    {
        Errors = identityResult.Errors
            .Select(x => x.Code + " - " + x.Description).ToArray();
    }

    public ApiBadRequestResponse(string message)
        : base(400, message)
    {
    }
}