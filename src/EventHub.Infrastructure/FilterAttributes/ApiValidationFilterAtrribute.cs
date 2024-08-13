﻿using EventHub.Shared.HttpResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventHub.Infrastructure.FilterAttributes;

public class ApiValidationFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
            context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(context.ModelState));

        base.OnActionExecuting(context);
    }
}