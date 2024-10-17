using FluentValidation;

namespace Application.Email.Commands.SendResetPasswordEmail;

public class SendResetPasswordEmailValidator : AbstractValidator<SendResetPasswordEmailCommand>
{
    public SendResetPasswordEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email to reset password is required!");
    }
}