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
        if(context.Exception is MyRecipeBookException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnknowException(context);
        }
    }

    public void HandleProjectException(ExceptionContext context)
    {
        if(context.Exception is ErrorOnValidationException)
        {
            var validationException = context.Exception as ErrorOnValidationException;
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            context.Result = 
                new BadRequestObjectResult(new ResponseErrorJson(validationException.ErrorMessages));
        }
    }

    public void ThrowUnknowException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result =
            new ObjectResult(new ResponseErrorJson("kabum"));
    }
}
