using Application.Contracts.AppointmentAnswer.Patients.Responses;
using ErrorOr;
using MediatR;

namespace Application.Appointment.Doctors.Queries.DisplayUndeterminedVisits;

public record DisplayUndeterminedVisitsQuery() : IRequest<ErrorOr<DisplayUndeterminedVisitsResponse>>;