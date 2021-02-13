using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using WorkTimeSheet.Excepions;

namespace WorkTimeSheet.Filters
{
    public class GenericExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var errorMessage = new ErrorMessage();
            if (context.Exception is InvalidUserException)
            {
                errorMessage.Message = string.IsNullOrEmpty(context.Exception.Message) ? "Invalid User" : context.Exception.Message;
                context.HttpContext.Response.StatusCode = 401;
            }
            else if (context.Exception is SecurityTokenException)
            {
                errorMessage.Message = string.IsNullOrEmpty(context.Exception.Message) ? "Invalid User" : context.Exception.Message;
                context.HttpContext.Response.StatusCode = 401;
            }
            else if (context.Exception is DataNotFoundException)
            {
                errorMessage.Message = string.IsNullOrEmpty(context.Exception.Message) ? "Not able to fetch required data" : context.Exception.Message;
                context.HttpContext.Response.StatusCode = 404;
            }
            else if (context.Exception is InternalServerException)
            {
                errorMessage.Message = string.IsNullOrEmpty(context.Exception.Message) ? "Internal Server Exception" : context.Exception.Message;
                context.HttpContext.Response.StatusCode = 500;
            }

            context.Result = new JsonResult(errorMessage);
            base.OnException(context);
        }
    }
}
