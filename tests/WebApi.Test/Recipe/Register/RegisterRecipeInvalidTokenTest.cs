using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.Recipe.Register;

public class RegisterRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string METHOD = "recipe";

    [Fact]
    public async Task RegisterRecipe_InvalidToken_ShouldBeUnauthorized()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();
        var invalidToken = "invalid_token";

        // Act
        var response = await DoPost(method: METHOD, request: request, token: invalidToken);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RegisterRecipe_MissingToken_ShouldBeUnauthorized()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();

        // Act
        var response = await DoPost(METHOD, request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RegisterRecipe_ExpiredToken_ShouldBeUnauthorized()
    {
        // Arrange
        var request = RequestRecipeJsonBuilder.Build();
        var expiredToken = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        // Act
        var response = await DoPost(method: METHOD, request: request, token: expiredToken);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

}
