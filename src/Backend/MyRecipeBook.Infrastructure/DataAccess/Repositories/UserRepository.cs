using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository(MyrecipeBookDbContext dbContext) : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    public async Task Add(User user) => await dbContext.Users.AddAsync(user);

    public async Task<bool> ExistsActiveUserWithEmail(string email) 
        => await dbContext.Users.AnyAsync(u => u.Email.Equals(email) && u.Active);

    public async Task<bool> ExistsActiveUserWithIdentifier(Guid userIdentifier)
        => await dbContext.Users.AnyAsync(u => u.UserIdentifier.Equals(userIdentifier) && u.Active);

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(
            user => user.Email.Equals(email) && user.Password.Equals(password) && user.Active);
    }
}
