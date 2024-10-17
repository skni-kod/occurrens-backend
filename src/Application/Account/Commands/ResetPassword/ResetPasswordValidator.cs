using FluentValidation;

namespace Application.Account.Commands.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id is required");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");

        RuleFor(x => x.RepeatPassword)
            .Equal(x => x.Password).NotEmpty()
            .WithMessage("Wrong password");
    }
}