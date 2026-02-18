using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public RegisterUserTest(CustomWebApplicationFactory factory)
        => _httpClient = factory.CreateClient();

    [Fact]
    public async Task PostUser_Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync("User", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var reponseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(reponseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace()
            .And.Be(request.Name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task PostUser_WhenNameIsEmpty_ShouldBeBadRequestWithNameEmptyError(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty; // Make invalid

        const string Language = "Accept-Language";
        if (_httpClient.DefaultRequestHeaders.Contains(Language) ) {
            _httpClient.DefaultRequestHeaders.Remove(Language);
        }
        _httpClient.DefaultRequestHeaders.Add(Language, culture);

        var response = await _httpClient.PostAsJsonAsync("User", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var reponseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(reponseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessageBody = ResourceMessagesException.ResourceManager.GetString(
            "EMPTY", new CultureInfo(culture));
        var expectedVariable = ResourceMessagesException.ResourceManager.GetString(
            "NAME", new CultureInfo(culture));
        var expectedMessage = string.Format(expectedMessageBody!, expectedVariable);

        errors.Should().NotBeEmpty();
        errors.Should().ContainSingle()
            .And
            .Contain(e => e.GetString()!.Equals(expectedMessage));

    }

}
