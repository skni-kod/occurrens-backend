using Application.Contracts.AppointmentAnswer.Patients.Responses;
using ErrorOr;
using MediatR;

namespace Application.Appointment.Patients.Queries.GetAllVisits;

public record GetAllVisitsQuery() : IRequest<ErrorOr<GetAllVisitsResponse>>;