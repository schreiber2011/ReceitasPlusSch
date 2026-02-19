using AutoMapper;
using Mapster;
using MyRecipeBook.Application.Services.Criptography;
using MyRecipeBook.Application.Services.Mappings;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using System.Threading.Tasks;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    IUserWriteOnlyRepository writeOnlyRepository,
    IUserReadOnlyRepository readOnlyRepository,
    IUnitOfWork unitOfWork,
    IMapper autoMapper,
    PasswordEncripter passwordEncripter
        ) : IRegisterUserUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = autoMapper.Map<Domain.Entities.User>(request);

        user.Password = passwordEncripter.Encrypt(request.Password);

        await writeOnlyRepository.Add(user);

        await unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = request.Name,

        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        var emailAlreadyExists = await readOnlyRepository.ExistsActiveUserWithEmail(request.Email);
        if (emailAlreadyExists)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(
                nameof(request.Email),
                ErrorMessages.EMAIL_INVALID));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
