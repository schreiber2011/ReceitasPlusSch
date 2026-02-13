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

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserWriteOlnyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _autoMapper;
    private readonly PasswordEncripter _passwordEncripter;

    public RegisterUserUseCase(
        IUserWriteOlnyRepository writeOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IUnitOfWork unitOfWork,
        IMapper autoMapper,
        PasswordEncripter passwordEncripter
        )
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
        _autoMapper = autoMapper;
        _passwordEncripter = passwordEncripter;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _autoMapper.Map<Domain.Entities.User>(request);
        //var user = request.Adapt<Domain.Entities.User>();
        //user.Password = "";

        user.Password = _passwordEncripter.Encrypt(request.Password);

        await _writeOnlyRepository.Add(user);

        await _unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = request.Name,

        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        var emailAlreadyExists = await _readOnlyRepository.ExistsActiveUserWithEmail(request.Email);
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
