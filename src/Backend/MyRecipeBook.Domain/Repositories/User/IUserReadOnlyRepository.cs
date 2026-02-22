namespace MyRecipeBook.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistsActiveUserWithEmail(string email);
    Task<bool> ExistsActiveUserWithIdentifier(Guid userIdentifier);
    public Task<Entities.User?> GetByEmailAndPassword(string email, string password);
}
