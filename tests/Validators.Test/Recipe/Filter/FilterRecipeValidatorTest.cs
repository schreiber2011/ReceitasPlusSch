using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe.Filter;

public class FilterRecipeValidatorTest
{
    [Fact]
    public void Validate_WhenAllFieldsAreValid_ShouldBeValid()
    {
        // Arrange
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenCookingTimeIsInvalid_ShouldBeInvalid()
    {
        // Arrange
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes!.Add((MyRecipeBook.Communication.Enums.CookingTime)1000);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
    }

    [Fact]
    public void Validate_WhenDifficultyIsInvalid_ShouldBeInvalid()
    {
        // Arrange
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.Difficulties!.Add((MyRecipeBook.Communication.Enums.Difficulty)1000);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
    }

    [Fact]
    public void Validate_WhenDishTypeIsInvalid_ShouldBeInvalid()
    {
        // Arrange
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.DishTypes!.Add((MyRecipeBook.Communication.Enums.DishType)1000);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
    }
}