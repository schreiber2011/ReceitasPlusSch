using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Filters;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.API.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFiltere))
    {
    }
}
