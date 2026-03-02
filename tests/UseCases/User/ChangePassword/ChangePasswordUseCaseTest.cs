using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Execute_WhenRequestIsValid_ShouldChangePassword()
    {
        // Arrange
        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = password;

        // Act
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        // Assert
        await act.Should().NotThrowAsync();
        var passwordEncripter = PasswordEncripterBuilder.Build();

        user.Password.Should().Be(passwordEncripter.Encrypt(request.NewPassword));
    }

    [Fact]
    public async Task Execute_WhenNewPasswordIsEmpty_ShouldThrowErrorOnValidationException()
    {
        // Arrange
        (var user, var password) = UserBuilder.Build();
        var request = new RequestChangePasswordJson
        {
            Password = password,
            NewPassword = string.Empty
        };

        // Act
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => await useCase.Execute(request);

        // Assert
        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 2 &&
            e.ErrorMessages.Contains(ErrorMessages.PASSWORD_EMPTY) &&
            e.ErrorMessages.Contains(ErrorMessages.PASSWORD_NOT6CHAR)
            );
    }

    [Fact]
    public async Task Execute_WhenPasswordIsIncorrect_ShouldThrowErrorOnValidationException()
    {
        // Arrange
        (var user, var _) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();

        // Act
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => await useCase.Execute(request);

        // Assert
        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
            e.ErrorMessages.Contains(ErrorMessages.PASSWORD_INVALID)
            );
    }

    private static ChangePasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncripter = PasswordEncripterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, passwordEncripter, userUpdateRepository, unitOfWork);
    }
}
