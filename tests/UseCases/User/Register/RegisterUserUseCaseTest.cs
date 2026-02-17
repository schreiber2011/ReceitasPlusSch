using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Register_WhenEmailAlreadyExists_ShouldThrowValidationException()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();

        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        readOnlyRepository.ExistActiveUserWithEmail(request.Email); // Simulate existing email
        
        var useCase = new RegisterUserUseCase(
            UserWriteOlnyRepositoryBuilder.Build(),
            readOnlyRepository.Build(),
            UnitOfWorkBuilder.Build(),
            MapperBuilder.Build(),
            PasswordEncripterBuilder.Build());

        // Act & Assert
        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(
            e => e.ErrorMessages.Count == 1
            &&
            e.ErrorMessages.Contains(ErrorMessages.EMAIL_INVALID));
        //var exception = await Assert.ThrowsAsync<ValidationException>(() => useCase.Execute(request));
        //exception.Errors.Should().ContainSingle(); // Only one error expected
        //exception.Should().HaveError(ErrorMessages.EMAIL_ALREADY_EXISTS);
    }

    [Fact]
    public async Task Register_WhenEmailIsEmpty_ShouldThrowValidationException()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty; // Set email to empty

        var useCase = new RegisterUserUseCase(
            UserWriteOlnyRepositoryBuilder.Build(),
            new UserReadOnlyRepositoryBuilder().Build(),
            UnitOfWorkBuilder.Build(),
            MapperBuilder.Build(),
            PasswordEncripterBuilder.Build());

        // Act & Assert
        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(
            e => e.ErrorMessages.Count == 1
            &&
            e.ErrorMessages.Contains(ErrorMessages.EMAIL_EMPTY));
        //var exception = await Assert.ThrowsAsync<ValidationException>(() => useCase.Execute(request));
        //exception.Errors.Should().ContainSingle(); // Only one error expected
        //exception.Should().HaveError(ErrorMessages.EMAIL_ALREADY_EXISTS);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = new RegisterUserUseCase(
            UserWriteOlnyRepositoryBuilder.Build(),
            new UserReadOnlyRepositoryBuilder().Build(),
            UnitOfWorkBuilder.Build(),
            MapperBuilder.Build(),
            PasswordEncripterBuilder.Build());

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrEmpty();
        result.Name.Should().Be(request.Name);
        result.Name.Should().Be(request.Name);
    }
}
