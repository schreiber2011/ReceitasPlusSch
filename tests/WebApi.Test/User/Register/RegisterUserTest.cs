using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string method = "user";

    [Fact]
    public async Task PostUser_Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await DoPost(method, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var reponseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(reponseBody);

        responseData.RootElement.GetProperty("name").GetString()
            .Should().NotBeNullOrWhiteSpace()
            .And.Be(request.Name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString()
            .Should().NotBeNullOrWhiteSpace();

    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task PostUser_WhenNameIsEmpty_ShouldBeBadRequestWithNameEmptyError(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty; // Make invalid

        var response = await DoPost(method, request, culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var reponseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(reponseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager
            .GetString("NAME_EMPTY", new CultureInfo(culture));


        errors.Should().NotBeEmpty();
        errors.Should().ContainSingle()
            .And
            .Contain(e => e.GetString()!.Equals(expectedMessage));

    }
}
