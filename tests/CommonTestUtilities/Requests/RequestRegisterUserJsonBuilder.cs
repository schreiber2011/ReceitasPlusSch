using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLength = 10)
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Email, (f, x) => f.Internet.Email(x.Name))
            .RuleFor(x => x.Password, f => f.Internet.Password(passwordLength))
            .Generate();
    }
}


