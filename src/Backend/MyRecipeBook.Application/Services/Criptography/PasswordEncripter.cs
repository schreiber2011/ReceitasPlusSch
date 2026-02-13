using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Application.Services.Criptography;

public class PasswordEncripter(string salt)
{
    public string Encrypt(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(salt + password);
        var hash = SHA512.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
