using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Validators.Test.Recipe;

public class RecipeValidatorTest
{
    [Fact]
    public void Validate_WhenAllFieldsAreValid_ShouldBeValid()
    {
        // Arrange
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenCookingTimeisNull_ShouldBeValid()
    {
        // Arrange
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTime = null;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenDifficultyIsNull_ShouldBeValid()
    {
        // Arrange
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Difficulty = null;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_When_DishTypesIsEmpty_ShouldBeValid()
    {
        // Arrange
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Clear();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenCookingTimeIsOutOfList_ShouldBeNotSupportedError()
    {
        // Arrange
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTime = (CookingTime?)1000;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
    }

    [Fact]
    public void Validate_WhenDifficultyIsOutOfList_ShouldBeNotSupportedError()
    {
        // Arrange
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Difficulty = (Difficulty?)1000;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("             ")]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Because it is a unit test")]
    public void Validate_WhenTitleIsEmpty_ShouldBeTitleEmptyError(string title)
    {
        // Arrange
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = title;

        // Act
        var result = validator.Validate(request);

        // Arange
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.RECIPE_TITLE_EMPTY));
    }

    [Fact]
    public void Validate_WhenIngredientsIsEmpty_ShouldBeAtLeastOneIngredientError()
    {
        // Arrange
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Clear();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT));
    }

    [Fact]
    public void Validate_WhenInstructionsIsEmpty_ShouldBeAtLeastOneInstructionError()
    {
        // Arrange
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.Clear();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION));
    }

    [Fact]
    public void Validate_WhenDishTypeIsInvalid_ShouldBeDishTypeNotSupportedError()
    {
        // Arrange
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Add((DishType)1000);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Because it is a unit test")]
    public void Validate_WhenOneIngredientIsEmpty_ShouldBeIngredientEmptyError(string ingredient)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Add(ingredient);

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
    }

    [Fact]
    public void Validate_WhenStepRepeatsId_ShouldBeInstructionsSameOrderError()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions[0].Step
            = request.Instructions[request.Instructions.Count - 1].Step;

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER));
    }

    [Fact]
    public void Validate_WhenStepIsNegative_ShouldNonNegativeStepNumberError()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions[0].Step = -1;

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.NON_NEGATIVE_INSTRUCTION_STEP));
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Because it is a unit test")]
    public void Validate_WhenTextIsEmpty_ShouldBeInstructionEmptyError(string instruction)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions[0].Text = instruction;

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EMPTY));
    }

    [Fact]
    public void Validate_WhenTextTooLong_ShouldBeInstructionTooLongError()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions[0].Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS));
    }
}
