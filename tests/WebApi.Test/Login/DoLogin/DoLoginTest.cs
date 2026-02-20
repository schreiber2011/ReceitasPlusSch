using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : MyRecipeBookClassFixture
{
    private readonly string method = "login";

    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GeEmail();
        _password = factory.GetPassword();
        _name = factory.GetName();
    }

    [Fact]
    public async Task PostLogin_Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };


        var response = await DoPost(method, request);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        await using var reponseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(reponseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace()
            .And.Be(_name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task PostLogin_WhenUserIsInvalid_ShouldBeBadRequestWithInvalidLoginError(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        var response = await DoPost(method, request, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var reponseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(reponseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = GetFormattedMessage("INVALIDS", "EMAIL", "PASSWORD", culture);

        errors.Should().NotBeEmpty();
        errors.Should().ContainSingle()
            .And
            .Contain(e => e.GetString()!.Equals(expectedMessage));

    }

    private static string GetFormattedMessage(string template, string variable0, string variable1, string culture)
    {
        var cultureInfo = new CultureInfo(culture);
        var templateMessage = ResourceMessagesException.ResourceManager.GetString(template, cultureInfo)!;
        var variableMessage0 = ResourceMessagesException.ResourceManager.GetString(variable0, cultureInfo)!;
        var variableMessage1 = ResourceMessagesException.ResourceManager.GetString(variable1, cultureInfo)!;
        return string.Format(templateMessage, variableMessage0, variableMessage1);
    }


}
