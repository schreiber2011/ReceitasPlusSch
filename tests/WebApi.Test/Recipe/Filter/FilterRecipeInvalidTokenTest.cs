using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.Recipe.Filter;

public class FilterRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string METHOD = "recipe/filter";

    [Fact]
    public async Task DoPost_WhenTokenIsInvalid_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = RequestFilterRecipeJsonBuilder.Build();

        // Act
        var response = await DoPost(method: METHOD, request: request, token: "tokenInvalid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DoPost_WhenTokenIsEmpty_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = RequestFilterRecipeJsonBuilder.Build();

        // Act
        var response = await DoPost(method: METHOD, request: request, token: string.Empty);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DoPost_WhenUserFromTokenIsNotFound_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = RequestFilterRecipeJsonBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        // Act
        var response = await DoPost(method: METHOD, request: request, token: token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}