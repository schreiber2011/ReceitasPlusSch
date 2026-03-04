using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Register;

public class RegisterRecipeUseCase(
    IRecipeWriteOnlyRepository repository,
    ILoggedUser _loggedUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRegisterRecipeUseCase
{
    public async Task<ResponseRegiteredRecipeJson> Execute(RequestRecipeJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();

        var recipe = mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = loggedUser.Id;

        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for (var index = 0; index < instructions.Count; index++)
            instructions[index].Step = index + 1;

        recipe.Instructions = mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

        await repository.Add(recipe);

        await unitOfWork.Commit();

        return mapper.Map<ResponseRegiteredRecipeJson>(recipe);
    }

    private static void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException([.. result.Errors
                .Select(e => e.ErrorMessage).Distinct()]);
    }
}
