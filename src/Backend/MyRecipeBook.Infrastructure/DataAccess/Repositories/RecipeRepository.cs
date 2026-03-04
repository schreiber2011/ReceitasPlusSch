using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(MyrecipeBookDbContext dbContext)
    : IRecipeWriteOnlyRepository
{
    public async Task Add(Recipe recipe) => await dbContext.Recipes.AddAsync(recipe);
}
