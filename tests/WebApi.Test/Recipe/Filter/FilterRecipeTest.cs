using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Filter;

public class FilterRecipeTest : MyRecipeBookClassFixture
{
    private const string METHOD = "recipe/filter";
    private readonly Guid _userIdentifier;
    private readonly string _recipeTitle;
    private readonly MyRecipeBook.Domain.Enums.Difficulty _recipedifficultyLevel;
    private readonly MyRecipeBook.Domain.Enums.CookingTime _recipeCookingTime;
    private readonly IList<MyRecipeBook.Domain.Enums.DishType> _recipeDishTypes;

    public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();

        _recipeTitle = factory.GetRecipeTitle();
        _recipeCookingTime = factory.GetRecipeCookingTime();
        _recipedifficultyLevel = factory.GetRecipeDifficulty();
        _recipeDishTypes = factory.GetDishTypes();
    }

    [Fact]
    public async Task DoPost_WhenFiltersMatchExistingRecipes_ShouldReturnOkWithRecipes()
    {
        // Arrange
        var request = new RequestFilterRecipeJson
        {
            CookingTimes = [(MyRecipeBook.Communication.Enums.CookingTime)_recipeCookingTime],
            Difficulties = [(MyRecipeBook.Communication.Enums.Difficulty)_recipedifficultyLevel],
            DishTypes = [.. _recipeDishTypes.Select(dishType => (MyRecipeBook.Communication.Enums.DishType)dishType)],
            RecipeTitle_Ingredient = _recipeTitle,
        };
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        // Act
        var response = await DoPost(method: METHOD, request: request, token: token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("recipes").EnumerateArray().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task DoPost_WhenNoRecipesMatchFilter_ShouldReturnNoContent()
    {
        // Arrange
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.RecipeTitle_Ingredient = "recipeDontExist";
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        // Act
        var response = await DoPost(method: METHOD, request: request, token: token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task DoPost_WhenCookingTimeIsInvalid_ShouldReturnBadRequestWithTranslatedError(string culture)
    {
        // Arrange
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes!.Add((MyRecipeBook.Communication.Enums.CookingTime)1000);
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        // Act
        var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture));
        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}