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

    public static DisplayVisitInfoDto DisplayVisitInfoAsDto(this Visit visit)
    {
        return new DisplayVisitInfoDto
        {
            Id = visit.Id,
            Date = (DateOnly)visit.Date,
            Time = (TimeOnly)visit.Time,
            Description = visit.Description,
            Price = (float)visit.Price
        };
    }
}