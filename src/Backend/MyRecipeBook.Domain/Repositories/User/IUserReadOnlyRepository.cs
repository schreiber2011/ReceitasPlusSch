namespace MyRecipeBook.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistsActiveUserWithEmail(string email);

    public Task<Entities.User?> GetByEmailAndPassword(string email, string password);
}
