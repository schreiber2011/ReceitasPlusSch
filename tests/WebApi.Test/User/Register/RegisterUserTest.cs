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

        SetCulture(_httpClient, culture);

        var response = await _httpClient.PostAsJsonAsync("User", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var reponseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(reponseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = GetFormattedMessage("EMPTY", "NAME", culture);

        errors.Should().NotBeEmpty();
        errors.Should().ContainSingle()
            .And
            .Contain(e => e.GetString()!.Equals(expectedMessage));

    }

    private static void SetCulture(HttpClient client, string culture)
    {
        client.DefaultRequestHeaders.Remove("Accept-Language");
        client.DefaultRequestHeaders.Add("Accept-Language", culture);
    }

    private static string GetFormattedMessage(string template, string variable, string culture)
    {
        var cultureInfo = new CultureInfo(culture);
        var templateMessage = ResourceMessagesException.ResourceManager.GetString(template, cultureInfo)!;
        var variableMessage = ResourceMessagesException.ResourceManager.GetString(variable, cultureInfo)!;
        return string.Format(templateMessage, variableMessage);
    }

}
