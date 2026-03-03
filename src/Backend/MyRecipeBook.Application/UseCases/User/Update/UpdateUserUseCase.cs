using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserUseCase(
    ILoggedUser _loggedUser,
    IUserUpdateOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository,
    IUnitOfWork unitOfWork
        ) : IUpdateUserUseCase
{

    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.User();

        await Validate(request, loggedUser.Email);

        var user = await repository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        repository.Update(user);

        await unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string loggedUserEmail)
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        if (loggedUserEmail != request.Email)
        {
            var userExist = await userReadOnlyRepository.ExistsActiveUserWithEmail(request.Email);
            if (userExist)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(
                    nameof(request.Email),
                    ResourceMessagesException.EMAIL_INVALID));
            }
        }
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
