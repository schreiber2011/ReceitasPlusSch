using MyRecipeBook.Application.Services.Criptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(IUserReadOnlyRepository repository, PasswordEncripter passwordEncripter) : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository repository = repository;
    private readonly PasswordEncripter passwordEncripter = passwordEncripter;

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await repository.GetByEmailAndPassword(
                request.Email,
                passwordEncripter.Encrypt(request.Password)
            ) ?? throw new InvalidLoginException();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name
        };
    }
}
