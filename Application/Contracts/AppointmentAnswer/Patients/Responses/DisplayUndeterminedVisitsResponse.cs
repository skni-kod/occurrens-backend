using Core.Appointment.DTOS;

namespace Application.Contracts.AppointmentAnswer.Patients.Responses;

public record DisplayUndeterminedVisitsResponse(List<UndeterminedVisitsDto> List);