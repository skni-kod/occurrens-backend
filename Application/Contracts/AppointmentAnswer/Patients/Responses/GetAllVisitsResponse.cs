using Core.Appointment.DTOS;

namespace Application.Contracts.AppointmentAnswer.Patients.Responses;

public record GetAllVisitsResponse(List<DisplayVisitInfoDto> Visits);