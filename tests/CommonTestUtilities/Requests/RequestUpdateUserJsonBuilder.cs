using Bogus;
using MyRecipeBook.Communication.Requests;

namespace Validators.User.Update;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Email, (f, x) => f.Internet.Email(x.Name))
            .Generate();
    }

}