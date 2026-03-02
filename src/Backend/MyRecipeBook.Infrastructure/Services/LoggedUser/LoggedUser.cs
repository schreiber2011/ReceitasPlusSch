using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRecipeBook.Infrastructure.Services.LoggedUser;

public class LoggedUser(
    MyrecipeBookDbContext dbContext,
    ITokenProvider tokenProvider) : ILoggedUser
{
    public async Task<User> User()
    {
        var token = tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(
            x => x.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(identifier);

        return await dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
    }
}
