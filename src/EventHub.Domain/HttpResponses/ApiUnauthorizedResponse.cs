using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventHub.Domain.HttpResponses;

public class ApiUnauthorizedResponse : ApiResponse
{
    public ApiUnauthorizedResponse(ModelStateDictionary modelState)
        : base(401)
    {
        if (modelState.IsValid) throw new ArgumentException("ModelState must be invalid", nameof(modelState));

        Errors = modelState.SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();
    }

    public ApiUnauthorizedResponse(IdentityResult identityResult)
        : base(401)
    {
        Errors = identityResult.Errors
            .Select(x => x.Code + " - " + x.Description).ToArray();
    }

    public ApiUnauthorizedResponse(string message)
        : base(401, message)
    {
    }
}