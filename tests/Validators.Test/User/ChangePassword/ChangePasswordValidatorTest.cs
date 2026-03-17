using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Validate_WhenAllFieldsAreValid_ShouldBeValid()
    {
        // Arrange
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build(); // Valid: Password not empty, NewPassword >=6

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenPasswordIsEmpty_ShouldBeInvalidWithPasswordEmptyError()
    {
        // Arrange
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty; // Make invalid

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_EMPTY));
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_PASSWORD));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_WhenNewPasswordIsTooShort_ShouldBeInvalidWithPasswordNot6CharError(int passwordLength)
    {
        // Arrange
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build(passwordLength);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_PASSWORD));
    }
}
