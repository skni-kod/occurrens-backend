using Application.Common.Errors;
using Application.Contracts.AppointmentAnswer.Patients.Responses;
using Core.Appointment.Repositories;
using Core.DataFromClaims.UserId;
using ErrorOr;
using MediatR;

namespace Application.Appointment.Patients.Queries.GetAllVisits;

public class GetAllVisitsHandler : IRequestHandler<GetAllVisitsQuery, ErrorOr<GetAllVisitsResponse>>
{
    private readonly IGetUserId _getUserId;
    private readonly IAppointmentRepository _appointmentRepository;

    public GetAllVisitsHandler(IGetUserId getUserId, IAppointmentRepository appointmentRepository)
    {
        _getUserId = getUserId;
        _appointmentRepository = appointmentRepository;
    }
    
    public async Task<ErrorOr<GetAllVisitsResponse>> Handle(GetAllVisitsQuery request, CancellationToken cancellationToken)
    {
        var patientId = _getUserId.UserId;

        var result = await _appointmentRepository.GetAllVisits((Guid)patientId, cancellationToken);

        if (result is null) return Errors.AppointmentErrors.NothingToDispaly;

        return new GetAllVisitsResponse(result);
    }
}