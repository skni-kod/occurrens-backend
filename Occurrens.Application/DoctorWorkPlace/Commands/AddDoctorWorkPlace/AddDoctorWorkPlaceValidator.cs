using FluentValidation;

namespace Occurrens.Application.DoctorWorkPlace.Commands.AddDoctorWorkPlace;

public class AddDoctorWorkPlaceValidator : AbstractValidator<AddDoctorWorkPlaceCommand>
{
    public AddDoctorWorkPlaceValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().NotNull().WithMessage("Pole ulicy jest wymagane!");

        RuleFor(x => x.BuildingNumber)
            .NotNull().WithMessage("Pole numeru domu jest wymagane");

        RuleFor(x => x.PostalCode)
            .NotNull().NotEmpty().WithMessage("Pole kodu pocztowego jest wymagane!")
            .Matches(@"^\d{2}-\d{3}$").WithMessage("Niepoprawny format kodu pocztowego. Wprowadź kod w formacie XX-XXX.");

        RuleFor(x => x.City)
            .NotNull().NotEmpty().WithMessage("Pole miasta jest wymagane!"); 
    }
}