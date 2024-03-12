using Application.Appointment.Patients.Commands.AppointmentToDoctor;
using Application.Appointment.Patients.Queries.GetAllVisits;
using Core.Account.enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AppointmentWithDoctor;

[Route("patient-appointment")]
[Authorize(Roles = nameof(UserRoles.Patient))]
public class PatientController : ApiController
{
    private readonly IMediator _mediator;

    public PatientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Make appointment with doctor
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> MakeAppointmentWithDoctor([FromBody] AppointmentToDoctorCommand command)
    {
        var response = await _mediator.Send(command);

        return response.Match(
            messageResponse => Ok(messageResponse),
            erros => Problem(erros)
            );
    }

    /// <summary>
    /// Display unstarted visits
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAllVisits()
    {
        var query = new GetAllVisitsQuery();

        var response = await _mediator.Send(query);

        return response.Match(
            visitsResponse => Ok(visitsResponse),
            errors => Problem(errors)
            );
    }
}