using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;

public class JwtTokenValidator(string signingKey) : JwtTokenHandler, IAccessTokenValidator
{
    public Guid ValidateAndGetUserIdentifier(string token)
    {
        var validatonParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = SecurityKey(signingKey),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, validatonParameters, out _);

        var userIdentifier = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value
            ?? throw new SecurityTokenException("Invalid token");

        return Guid.Parse(userIdentifier);
    }
}
