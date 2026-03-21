using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase(
    ILoggedUser _loggedUser,
    IPasswordEncripter passwordEncripter,
    IUserUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork
        ) : IChangePasswordUseCase
{
    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.User();

        Validate(request, loggedUser);

        var user = await repository.GetById(loggedUser.Id);

        user.Password = passwordEncripter.Encrypt(request.NewPassword);

        repository.Update(user);

        await unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var currentPasswordEncrypted = passwordEncripter.Encrypt(request.Password);

        if (!currentPasswordEncrypted.Equals(loggedUser.Password))
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(
                string.Empty,
                ResourceMessagesException.INVALID_PASSWORD));
        }

        if (!result.IsValid)
        {
            throw new ErrorOnValidationException([.. result.Errors.Select(
                e => e.ErrorMessage)]);
        }
    }
}
