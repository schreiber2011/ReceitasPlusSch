using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage(ErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ErrorMessages.EMAIL_EMPTY);
        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email)
                .EmailAddress()
                .WithMessage(ErrorMessages.EMAIL_INVALID);
        });
        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage(ErrorMessages.PASSWORD_EMPTY)
            .MinimumLength(6)
            .WithMessage(ErrorMessages.PASSWORD_NOT6CHAR);

    }
}
