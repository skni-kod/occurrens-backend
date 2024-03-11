using Core.Appointment.DTOS;
using Core.Appointment.Extensions;
using Core.Appointment.Repositories;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using occurrensBackend.Entities.DatabaseEntities;

namespace Infrastructure.Appointment.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly OccurrensDbContext _context;

    public AppointmentRepository(OccurrensDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsDoctorExist(Guid doctorId, CancellationToken cancellationToken)
    {
        return await _context.Doctors.AnyAsync(x => x.Id == doctorId, cancellationToken);
    }

    public async Task MakeAppointmentWithDoctor(Visit visit, CancellationToken cancellationToken)
    {
        await _context.Visits.AddAsync(visit,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> SetDateOfVisit(Guid doctorId, Guid visitId, SetVisitInfoDto dto, CancellationToken cancellationToken)
    {
        var visit = await _context.Visits.FindAsync(visitId, cancellationToken);

        if (visit is null || visit.DoctorId != doctorId) return false;

        visit.Date = dto.Date;
        visit.Time = dto.Time;
        visit.Price = dto.Price;
        visit.Is_estabilished = true;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<List<UndeterminedVisitsDto>> GetUndeterminedVisits(Guid doctorId, CancellationToken cancellationToken)
    {
        var visits = await _context.Visits.Where(x => x.DoctorId == doctorId && !x.Is_estabilished)
            .ToListAsync(cancellationToken);

        if (visits is null) return null;

        var result = visits.Select(x => x.UndeterminedVisitsAsDto()).ToList();

        return result;
    }
}