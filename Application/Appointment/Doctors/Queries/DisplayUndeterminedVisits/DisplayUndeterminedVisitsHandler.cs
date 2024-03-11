using Application.Common.Errors;
using Application.Contracts.AppointmentAnswer.Patients.Responses;
using Core.Appointment.Repositories;
using Core.DataFromClaims.UserId;
using ErrorOr;
using MediatR;

namespace Application.Appointment.Doctors.Queries.DisplayUndeterminedVisits;

public class DisplayUndeterminedVisitsHandler : IRequestHandler<DisplayUndeterminedVisitsQuery, ErrorOr<DisplayUndeterminedVisitsResponse>>
{
    private readonly IGetUserId _getUserId;
    private readonly IAppointmentRepository _appointmentRepository;

    public DisplayUndeterminedVisitsHandler(IGetUserId getUserId, IAppointmentRepository appointmentRepository)
    {
        _getUserId = getUserId;
        _appointmentRepository = appointmentRepository;
    }
    
    public async Task<ErrorOr<DisplayUndeterminedVisitsResponse>> Handle(DisplayUndeterminedVisitsQuery request, CancellationToken cancellationToken)
    {
        var doctorId = _getUserId.UserId;

        var result = await _appointmentRepository.GetUndeterminedVisits((Guid)doctorId, cancellationToken);

        if (result == null) return Errors.AppointmentErrors.NothingToDispaly;

        return new DisplayUndeterminedVisitsResponse(result);
    }
}