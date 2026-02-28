using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ErrorMessages.NAME_EMPTY);
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ErrorMessages.EMAIL_EMPTY);
        When(user => !string.IsNullOrWhiteSpace(user.Email), () =>
        {
            RuleFor(user => user.Email)
                .EmailAddress()
                .WithMessage(ErrorMessages.EMAIL_INVALID);
        });

    }
}
