using FluentValidation;

namespace Application.Account.Commands.ConfirmAccount;

public class ConfirmAccountValidator : AbstractValidator<ConfirmAccountCommand>
{
    public ConfirmAccountValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required!");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User is required!");
    }
}