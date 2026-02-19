using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions.ExceptionsBase;
using System.Net;

namespace MyRecipeBook.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MyRecipeBookException)
            HandleProjectException(context);
        else
            ThrowUnknownException(context);
    }

    public static void HandleProjectException(ExceptionContext context)
    {
        if (context.Exception is ErrorOnValidationException validationException)
        {
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            context.Result = 
                new BadRequestObjectResult(new ResponseErrorJson(validationException.ErrorMessages));
        }
    }

    public static void ThrowUnknownException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result =
            new ObjectResult(new ResponseErrorJson("kabum"));
    }
}
