using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;

namespace Validators.User.Register;

public class RegisterUserValidatorTest
{
    private readonly RegisterUserValidator _validator;

    // Constructor replaces [SetUp] for initialization
    public RegisterUserValidatorTest()
    {
        _validator = new RegisterUserValidator();
    }

    // Positive Test: All fields valid
    [Fact]
    public void Validate_WhenAllFieldsAreValid_ShouldBeValid()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build(); // Valid: Name not empty, Email valid, Password >=6

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    // Negative Test: Name empty
    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldBeInvalidWithNameEmptyError()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty; // Make invalid

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
    }

    // Negative Test: Email empty
    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldBeInvalidWithEmailEmptyError()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty; // Make invalid

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
    }

    // Negative Test: Email invalid format
    [Fact]
    public void Validate_WhenEmailIsInvalid_ShouldBeInvalidWithEmailInvalidError()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "invalid-email"; // Make invalid (no @ or domain)

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
    }

    // Negative Test: Password empty
    [Fact]
    public void Validate_WhenPasswordIsEmpty_ShouldBeInvalidWithPasswordEmptyError()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty; // Make invalid

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_EMPTY));
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_PASSWORD));
    }

    // Negative Test: Password too short
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_WhenPasswordIsTooShort_ShouldBeInvalidWithPasswordNot6CharError(
        int passwordLength)
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one error expected
        result.Errors.Should().Contain(
            e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_PASSWORD));
    }

}
