using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserWriteOlnyRepositoryBuilder
{
    public static IUserWriteOlnyRepository Build() 
        => new Mock<IUserWriteOlnyRepository>().Object;
}
