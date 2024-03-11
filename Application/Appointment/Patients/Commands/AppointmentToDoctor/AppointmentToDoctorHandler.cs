using Application.Common.Errors;
using Application.Contracts.AppointmentAnswer.Patients.Responses;
using Core.Appointment.Repositories;
using Core.DataFromClaims.UserId;
using ErrorOr;
using MediatR;
using occurrensBackend.Entities.DatabaseEntities;

namespace Application.Appointment.Patients.Commands.AppointmentToDoctor;

public class AppointmentToDoctorHandler : IRequestHandler<AppointmentToDoctorCommand, ErrorOr<AppointmentMessageResponse>>
{
    private readonly IGetUserId _getUserId;
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentToDoctorHandler(IGetUserId getUserId, IAppointmentRepository appointmentRepository)
    {
        _getUserId = getUserId;
        _appointmentRepository = appointmentRepository;
    }    
    
    public async Task<ErrorOr<AppointmentMessageResponse>> Handle(AppointmentToDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctorExists = await _appointmentRepository.IsDoctorExist(request.DoctorId, cancellationToken);

        if (!doctorExists) return Errors.AppointmentErrors.DoctorDoesntExist;
        
        var userId = _getUserId.UserId;

        var newVisit = new Visit
        {
            Description = request.Description,
            DoctorId = request.DoctorId,
            PatientId = (Guid)userId
        };

        await _appointmentRepository.MakeAppointmentWithDoctor(newVisit, cancellationToken);

        return new AppointmentMessageResponse("Przesłano prośbę o wyzytę do lekarza");
    }
}