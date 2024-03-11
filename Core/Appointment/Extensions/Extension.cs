using Core.Appointment.DTOS;
using occurrensBackend.Entities.DatabaseEntities;

namespace Core.Appointment.Extensions;

public static class Extension
{
    public static UndeterminedVisitsDto UndeterminedVisitsAsDto(this Visit visit)
    {
        return new UndeterminedVisitsDto
        {
            Id = visit.Id,
            Description = visit.Description
        };
    }
}