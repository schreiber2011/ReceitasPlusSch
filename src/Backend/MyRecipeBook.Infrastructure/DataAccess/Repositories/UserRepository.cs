using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository(MyrecipeBookDbContext dbContext) : IUserWriteOlnyRepository, IUserReadOnlyRepository
{
    public async Task Add(User user) => await dbContext.Users.AddAsync(user);

    public async Task<bool> ExistsActiveUserWithEmail(string email) 
        => await dbContext.Users.AnyAsync(u => u.Email.Equals(email) && u.Active);
}
