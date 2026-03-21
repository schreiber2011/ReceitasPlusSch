using MyRecipeBook.Domain.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Cryptography;

public class Sha512Encripter(string salt) : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(salt + password);
        var hash = SHA512.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
