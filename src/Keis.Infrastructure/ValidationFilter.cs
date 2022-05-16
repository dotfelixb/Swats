using Keis.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Keis.Infrastructure;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // TODO Review this for Web Api
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            var errorResult = new ErrorResult
            {
                Ok = false,
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(errorResult);
            return;
        }

        await next();
    }
}
