using Core.Appointment.DTOS;
using Core.Appointment.Extensions;
using Core.Appointment.Repositories;
using Core.Date;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using occurrensBackend.Entities.DatabaseEntities;

namespace Infrastructure.Appointment.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly OccurrensDbContext _context;
    private readonly IDateService _dateService;

    public AppointmentRepository(OccurrensDbContext context, IDateService dateService)
    {
        _context = context;
        _dateService = dateService;
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

    public async Task<List<DisplayVisitInfoDto>> GetAllVisits(Guid patientId, CancellationToken cancellationToken)
    {
        var visits = await _context.Visits.Where(x => x.PatientId == patientId && 
                                                      x.Is_estabilished && 
                                                      x.Date >= _dateService.CurrentDate())
            .ToListAsync(cancellationToken);

        if (visits is null) return null;

        return visits.Select(x => x.DisplayVisitInfoAsDto()).ToList();
    }
}