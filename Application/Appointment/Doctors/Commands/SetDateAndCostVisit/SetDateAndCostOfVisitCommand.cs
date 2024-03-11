using Application.Contracts.AppointmentAnswer.Patients.Responses;
using ErrorOr;
using MediatR;

namespace Application.Appointment.Doctors.Commands.SetDateAndCostVisit;

public record SetDateAndCostOfVisitCommand(
    Guid Id,
    DateOnly Date,
    TimeOnly Time,
    float Price
    ) : IRequest<ErrorOr<SetDateOfVisitResponse>>;