using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;

namespace Validators.User.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Validate_WhenAllFieldsAreValid_ShouldBeValid()
    {
        // Arrange
        var _validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build(); // Valid: Name not empty, Email valid, Password >=6
        // Act
        var result = _validator.Validate(request);
        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldBeInvalidWithNameEmptyError()
    {
        // Arrange
        var _validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty; // Make invalid
        // Act
        var result = _validator.Validate(request);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
    }

    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldBeInvalidWithEmailEmptyError()
    {
        // Arrange
        var _validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty; // Make invalid
        // Act
        var result = _validator.Validate(request);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
    }

    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldBeInvalidWithEmailInvalidError()
    {
        // Arrange
        var _validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "email.com"; // Make invalid
        // Act
        var result = _validator.Validate(request);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
    }

}
