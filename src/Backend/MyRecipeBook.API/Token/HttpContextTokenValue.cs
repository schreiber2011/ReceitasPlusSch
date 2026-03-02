using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Token
{
    public class HttpContextTokenValue(
        IHttpContextAccessor httpContextAccessor) : ITokenProvider
    {
        public string Value()
        {
            var authentication = httpContextAccessor.HttpContext!.Request.Headers
                .Authorization.ToString();
            if (string.IsNullOrWhiteSpace(authentication)
            || !authentication.StartsWith("Bearer "))
            {
                throw new MyRecipeBookException(ErrorMessages.NO_TOKEN);
            }
            return authentication.Substring("Bearer ".Length).Trim();

        }
    }
}
