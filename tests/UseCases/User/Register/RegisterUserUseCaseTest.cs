using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
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

        RegisterUserUseCase useCase = GetRegisterUserUseCase(readOnlyRepository);

        // Act & Assert
        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(
            e => e.ErrorMessages.Count == 1
            &&
            e.ErrorMessages.Contains(ErrorMessages.EMAIL_INVALID));
    }

    [Fact]
    public async Task Register_WhenEmailIsEmpty_ShouldThrowValidationException()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty; // Set email to empty

        var useCase = GetRegisterUserUseCase(new UserReadOnlyRepositoryBuilder());

        // Act & Assert
        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(
            e => e.ErrorMessages.Count == 1
            &&
            e.ErrorMessages.Contains(ErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public async Task Register_Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = GetRegisterUserUseCase(new UserReadOnlyRepositoryBuilder());

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrEmpty();
        result.Name.Should().Be(request.Name);
        result.Tokens.Should().NotBeNull();
        result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
    }

    private static RegisterUserUseCase GetRegisterUserUseCase(UserReadOnlyRepositoryBuilder readOnlyRepository)
    {
        return new RegisterUserUseCase(
            UserWriteOnlyRepositoryBuilder.Build(),
            readOnlyRepository.Build(),
            UnitOfWorkBuilder.Build(),
            MapperBuilder.Build(),
            JwtTokenGeneratorBuilder.Build(),
            PasswordEncripterBuilder.Build());
    }
}
