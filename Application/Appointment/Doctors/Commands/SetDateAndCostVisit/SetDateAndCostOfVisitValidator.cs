using FluentValidation;

namespace Application.Appointment.Doctors.Commands.SetDateAndCostVisit;

public class SetDateAndCostOfVisitValidator : AbstractValidator<SetDateAndCostOfVisitCommand>
{
    public SetDateAndCostOfVisitValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().NotNull().WithMessage("Należy wybrać wizytę którą chcesz umówić");

        RuleFor(x => x.Date)
            .NotEmpty().NotNull().WithMessage("Należy przydzielić datę wizyty");

        RuleFor(x => x.Date)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Data wizyty musi być większa lub równa dzisiejszej");


        RuleFor(x => x.Time)
            .NotEmpty().NotNull().WithMessage("Należy przydzielić godzinę wizyty");

        RuleFor(x => x.Time)
            .Must(time => time >= TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay))
            .When(x => x.Date == DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Godzina wizyty musi być większa lub równa aktualnej godzinie");

        RuleFor(x => x.Price)
            .NotEmpty().NotNull().WithMessage("Podaj potencjalne koszty leczenia");
    }
}