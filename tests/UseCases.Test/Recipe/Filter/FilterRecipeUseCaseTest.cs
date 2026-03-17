using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.Recipe.Filter;

public class FilterRecipeUseCaseTest
{
    [Fact]
    public async Task Execute_WhenRequestIsValid_ShouldReturnFilteredRecipes()
    {
        // Arrange
        (var user, _) = UserBuilder.Build();
        var request = RequestFilterRecipeJsonBuilder.Build();
        var recipes = RecipeBuilder.Collection(user);
        var useCase = CreateUseCase(user, recipes);

        // Act
        var result = await useCase.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.Recipes.Should().NotBeNullOrEmpty();
        result.Recipes.Should().HaveCount(recipes.Count);
    }

    [Fact]
    public async Task Execute_WhenCookingTimeIsInvalid_ShouldThrowErrorOnValidationException()
    {
        // Arrange
        (var user, _) = UserBuilder.Build();
        var recipes = RecipeBuilder.Collection(user);
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes!.Add((MyRecipeBook.Communication.Enums.CookingTime)1000);
        var useCase = CreateUseCase(user, recipes);

        // Act
        Func<Task> act = async () => { await useCase.Execute(request); };

        // Assert
        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
                e.ErrorMessages.Contains(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
    }

    private static FilterRecipeUseCase CreateUseCase(
        MyRecipeBook.Domain.Entities.User user,
        IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeReadOnlyRepositoryBuilder().Filter(user, recipes).Build();

        return new FilterRecipeUseCase(mapper, repository, loggedUser);
    }
}