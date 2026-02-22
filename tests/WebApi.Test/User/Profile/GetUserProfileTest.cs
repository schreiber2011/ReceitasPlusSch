using Azure.Core;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "user";

    [Fact]
    public async Task GetUserProfile_Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIdentifier());

        var response = await DoGet(METHOD, token);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString()
            .Should().NotBeNullOrWhiteSpace()
            .And.Be(factory.GetName());
        responseData.RootElement.GetProperty("email").GetString()
            .Should().NotBeNullOrWhiteSpace()
            .And.Be(factory.GetEmail());
    }

    [Fact]
    public async Task GetUserProfile_WhenTokenInvalid_ShouldBeUnauthorized()
        {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet(METHOD, token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetUserProfile_WhenTokenIsEmpty_ShouldBeUnauthorized()
    {
        var response = await DoGet(METHOD, string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
