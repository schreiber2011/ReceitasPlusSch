namespace MyRecipeBook.Domain.Repositories.User;

public interface IUserWriteOlnyRepository
{
    public Task Add(Entities.User user);
}
