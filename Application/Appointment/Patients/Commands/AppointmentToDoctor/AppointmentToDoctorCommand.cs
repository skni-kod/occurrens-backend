using Application.Contracts.AppointmentAnswer.Patients.Responses;
using ErrorOr;
using MediatR;

namespace Application.Appointment.Patients.Commands.AppointmentToDoctor;

public record AppointmentToDoctorCommand(
    Guid DoctorId,
    string Description
    ) : IRequest<ErrorOr<AppointmentMessageResponse>>;