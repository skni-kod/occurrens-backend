using FluentValidation;

namespace Application.Account.Commands.SignUp;

public class SignUpValidator : AbstractValidator<SignUpCommand>
{
    public SignUpValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required!");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telephone number is required!")
            .Matches(@"^\d{9}$").WithMessage("Telephone number must be exactly 9 digits.");

        RuleFor(x => x.Pesel)
            .NotEmpty().WithMessage("Pesel is required!")
            .Matches(@"^\d{11}$").WithMessage("Pesel must be exactly 11 digits.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Dirth date is required!");

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress().WithMessage("Email is required!");

        RuleFor(x => x.RepeatPassword)
            .Equal(x => x.Password).WithMessage("Password not equal!");
    }
}