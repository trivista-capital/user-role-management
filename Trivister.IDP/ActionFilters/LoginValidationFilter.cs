using Microsoft.AspNetCore.Mvc.Filters;

namespace Trivister.IDP.ActionFilters;

public class LoginValidationFilter: ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        //base.OnActionExecuted(context);
    }
}