using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public class FilterRecipeUseCase(
    IMapper mapper,
    IRecipeReadOnlyRepository repository,
    ILoggedUser loggedUser) : IFilterRecipeUseCase
{

    public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request)
    {
        Validate(request);

        var _loggedUser = await loggedUser.User();

        var filters = new Domain.Dtos.FilterRecipesDto
        {
            RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
            CookingTimes = [.. (request.CookingTimes?? []).Distinct().Select(
                c => (Domain.Enums.CookingTime)c)],
            Difficulties = [.. (request.Difficulties?? []).Distinct().Select(
                c => (Domain.Enums.Difficulty)c)],
            DishTypes = [.. (request.DishTypes?? []).Distinct().Select(
                c => (Domain.Enums.DishType)c)]
        };

        var recipes = await repository.Filter(_loggedUser, filters);

        return new ResponseRecipesJson
        {
            Recipes = mapper.Map<List<ResponseShortRecipeJson>>(recipes)
        };
    }

    private static void Validate(RequestFilterRecipeJson request)
    {
        var validator = new FilterRecipeValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(
                error => error.ErrorMessage).Distinct().ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
