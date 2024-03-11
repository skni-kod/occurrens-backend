using Application.Common.Errors;
using Application.Contracts.AppointmentAnswer.Patients.Responses;
using Core.Appointment.DTOS;
using Core.Appointment.Repositories;
using Core.DataFromClaims.UserId;
using ErrorOr;
using MediatR;

namespace Application.Appointment.Doctors.Commands.SetDateAndCostVisit;

public class SetDateAndCostOfVisitHandler : IRequestHandler<SetDateAndCostOfVisitCommand, ErrorOr<SetDateOfVisitResponse>>
{
    private readonly IGetUserId _getUserId;
    private readonly IAppointmentRepository _appointmentRepository;

    public SetDateAndCostOfVisitHandler(IGetUserId getUserId, IAppointmentRepository appointmentRepository)
    {
        _getUserId = getUserId;
        _appointmentRepository = appointmentRepository;
    }
    
    public async Task<ErrorOr<SetDateOfVisitResponse>> Handle(SetDateAndCostOfVisitCommand request, CancellationToken cancellationToken)
    {
        var doctorId = _getUserId.UserId;

        var setDateOfVisit = new SetVisitInfoDto
        {
            Date = request.Date,
            Time = request.Time,
            Price = request.Price
        };

        var hasDoctorAccess =
            await _appointmentRepository.SetDateOfVisit((Guid)doctorId, request.Id, setDateOfVisit, cancellationToken);

        if (!hasDoctorAccess) return Errors.AppointmentErrors.HasNoAccess;

        return new SetDateOfVisitResponse(setDateOfVisit);
    }
}