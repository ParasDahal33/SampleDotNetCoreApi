using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BusReservationSystemApi.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //before controller
            if (!context.ModelState.IsValid)
            {
                // Only show one error message at a time. 
                var firstError = context.ModelState.FirstOrDefault().Value.Errors.FirstOrDefault();

                context.Result = new BadRequestObjectResult(firstError.ErrorMessage);
                return;
            }
            await next();

            //after controller
        }
    }
}
