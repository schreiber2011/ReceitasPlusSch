using MyRecipeBook.Application.Services.Criptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncripterBuilder
{
    public static PasswordEncripter Build() => new("salt");
}
