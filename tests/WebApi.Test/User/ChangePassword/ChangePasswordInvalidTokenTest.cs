using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using System.Net;

namespace WebApi.Test.User.ChangePassword;

public class ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    public const string METHOD = "user/change-password";

    [Fact]
    public async Task ChangePassword_InvalidToken_ShouldBeUnauthorized()
    {
        // Arrange
        var request = new RequestChangePasswordJson();
        var invalidToken = "invalid_token";

        // Act
        var response = await DoPut(METHOD, request, invalidToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangePassword_MissingToken_ShouldBeUnauthorized()
    {
        // Arrange
        var request = new RequestChangePasswordJson();

        // Act
        var response = await DoPut(METHOD, request, string.Empty);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangePassword_ExpiredToken_ShouldBeUnauthorized()
    {
        // Arrange
        var expiredToken = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var request = new RequestChangePasswordJson();

        // Act
        var response = await DoPut(METHOD, request, expiredToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
