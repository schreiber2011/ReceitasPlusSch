using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.User.ChangePassword;

public class ChangePasswordTest : MyRecipeBookClassFixture
{
    public const string METHOD = "user/change-password";

    public readonly string _password;
    public readonly string _email;
    public readonly Guid _userIdentifier;

#pragma warning disable IDE0290 // Use primary constructor (DO NOT CHANGE)
    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _password = factory.GetPassword();
        _email = factory.GetEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }
#pragma warning restore IDE0290 // Use primary constructor

    [Fact]
    public async Task ChangePassword_Success()
    {
        // Arrange - Scene 1
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        // Act - Scene 1
        var response = await DoPut(METHOD, request, token);

        // Assert - Scene 1
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Arrange - Scene 2
        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        // Act - Scene 2
        response = await DoPost("login", loginRequest);

        // assert - Scene 2
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        // Arrange - Scene 3
        loginRequest.Password = request.NewPassword;

        // Act - Scene 3
        response = await DoPost("login", loginRequest);

        // Assert - Scene 3
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ChangePassword_WhenNewPasswordIsEmpty_ShouldBeBadRequestWithPasswordEmptyError()
    {
        // Arrange
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty; // Make invalid

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        // Act
        var response = await DoPut(METHOD, request, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ErrorMessages.PASSWORD_EMPTY;

        errors.Should().ContainSingle(e => e.GetString() == expectedMessage);
    }
}