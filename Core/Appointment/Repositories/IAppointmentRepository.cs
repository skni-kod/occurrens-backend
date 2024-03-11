using Core.Appointment.DTOS;
using occurrensBackend.Entities.DatabaseEntities;

namespace Core.Appointment.Repositories;

public interface IAppointmentRepository
{
    Task<bool> IsDoctorExist(Guid doctorId, CancellationToken cancellationToken);
    Task MakeAppointmentWithDoctor(Visit visit, CancellationToken cancellationToken);
    Task<bool> SetDateOfVisit(Guid doctorId, Guid visitId, SetVisitInfoDto dto, CancellationToken cancellationToken);
}