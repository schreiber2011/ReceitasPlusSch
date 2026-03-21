using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Validators.Test.User.Update;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Execute_WhenRequestIsValid_ShouldUpdateUser()
    {
        // Arrange
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build(); // Valid request

        var _useCase = CreateUseCase(user);

        // Act
        Func<Task> act = async () => await _useCase.Execute(request);

        // Assert
        await act.Should().NotThrowAsync(); // Should not throw any exceptions

        user.Email.Should().Be(request.Email);
        user.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Execute_WhenNameIsEmpty_ShouldThrowValidationException()
    {
        // Arrange
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty; // Set name to empty

        var useCase = CreateUseCase(user);

        // Act
        Func<Task> act = async () => await useCase.Execute(request);

        // Assert
        await act.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.ErrorMessages.Count == 1 &&
            e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));
    }

    [Fact]
    public async Task Execute_WhenEmailIsEmpty_ShouldThrowValidationException()
    {
        // Arrange
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty; // Set email to empty

        var useCase = CreateUseCase(user);

        // Act
        Func<Task> act = async () => await useCase.Execute(request);

        // Assert
        await act.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.ErrorMessages.Count == 1 &&
            e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_EMPTY));
    }

    [Fact]
    public async Task Execute_WhenEmailAlreadyExists_ShouldThrowValidationException()
    {
        // Arrange
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        // Act
        Func<Task> act = async () => await useCase.Execute(request);

        // Assert
        await act.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.ErrorMessages.Count == 1 &&
            e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_INVALID));
    }

    private static UpdateUserUseCase CreateUseCase(
        MyRecipeBook.Domain.Entities.User user,
        string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        if (!string.IsNullOrWhiteSpace(user.Email))
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email!);

        return new UpdateUserUseCase(
            loggedUser,
            userUpdateRepository,
            userReadOnlyRepositoryBuilder.Build(),
            unitOfWork);
    }
}