using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Filters;

public class AuthenticatedUserFiltere(
    IAccessTokenValidator accessTokenValidator,
    IUserReadOnlyRepository userReadOnlyRepository) : IAsyncAuthorizationFilter
{

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnrequest(context);

            var userIdentifier = accessTokenValidator.ValidateAndGetUserIdentifier(token);

            var exist = await userReadOnlyRepository
                .ExistsActiveUserWithIdentifier(userIdentifier);
            if (!exist)
            {
                throw new MyRecipeBookException(ErrorMessages.NO_TOKEN);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(
                new ResponseErrorJson("token broky")
                {
                    TokenIsExpire = true,
                });
        }
        catch (MyRecipeBookException ex)
        {
            context.Result = new UnauthorizedObjectResult(
                new ResponseErrorJson(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(
                new ResponseErrorJson(ErrorMessages.TOKEN_MESSAGE_NOT_IMPLEMENTED));
        }
    }

    private static string TokenOnrequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authentication)
            || !authentication.StartsWith("Bearer "))
        {
            throw new MyRecipeBookException(ErrorMessages.NO_TOKEN);
        }
        return authentication.Substring("Bearer ".Length).Trim();
    }

}
