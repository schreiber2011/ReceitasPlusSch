using CommonTestUtilities.Tokens;
using FluentAssertions;
using Validators.Test.User.Update;

namespace WebApi.Test.User.Update;

public class UpdateuserInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string METHOD = "user";

    [Fact]
    public async Task PutUser_InvalidToken_ShouldBeUnauthorized()
    {
        // Arrange
        var request = RequestUpdateUserJsonBuilder.Build();
        var invalidToken = "invalid_token";

        // Act
        var response = await DoPut(METHOD, request, invalidToken);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PutUser_MissingToken_ShouldBeUnauthorized()
    {
        // Arrange
        var request = RequestUpdateUserJsonBuilder.Build();

        // Act
        var response = await DoPut(METHOD, request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PutUser_ExpiredToken_ShouldBeUnauthorized()
    {
        // Arrange
        var request = RequestUpdateUserJsonBuilder.Build();
        var expiredToken = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        // Act
        var response = await DoPut(METHOD, request, expiredToken);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}
