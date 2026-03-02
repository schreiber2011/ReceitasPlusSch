using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.SharedValidators;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
            errors.Add(ErrorMessages.PASSWORD_EMPTY);

        if (password.Length < 6)
            errors.Add(ErrorMessages.PASSWORD_NOT6CHAR);

        foreach (var error in errors)
            context.AddFailure(new ValidationFailure(context.PropertyPath, error));

        return true;
    }

    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
}
