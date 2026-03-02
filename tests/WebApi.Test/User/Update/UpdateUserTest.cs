using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using Validators.User.Update;

namespace WebApi.Test.User.Update;

public class UpdateUserTest : MyRecipeBookClassFixture
{
    private const string METHOD = "user";

    private readonly Guid _userIdentifier;


#pragma warning disable IDE0290 // Use primary constructor (DO NOT CHANGE)
    public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
        => _userIdentifier = factory.GetUserIdentifier();
#pragma warning restore IDE0290 // Use primary constructor

    [Fact]
    public async Task PutUser_Success()
    {
        // Arrange
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        // Act
        var response = await DoPut(METHOD, request, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task PutUser_WhenNameIsEmpty_ShouldBeBadRequestWithNameEmptyError()
    {
        // Arrange
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty; // Make invalid
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        // Act
        var response = await DoPut(METHOD, request, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutUser_WhenEmailIsEmpty_ShouldBeBadRequestWithEmailEmptyError()
    {
        // Arrange
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty; // Make invalid
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        // Act
        var response = await DoPut(METHOD, request, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutUser_WhenEmailIsInvalid_ShouldBeBadRequestWithEmailInvalidError()
    {
        // Arrange
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "email.com"; // Make invalid
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        // Act
        var response = await DoPut(METHOD, request, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}