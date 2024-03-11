using Application.Appointment.Doctors.Commands.SetDateAndCostVisit;
using Core.Account.enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AppointmentWithDoctor;

[Route("doctor-appointment")]
[Authorize(Roles = nameof(UserRoles.Doctor))]
public class DoctorController : ApiController
{
    private readonly IMediator _mediator;

    public DoctorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Set date and potential cost of visit
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> SetDateAndCostOfVisit([FromBody] SetDateAndCostOfVisitCommand command)
    {
        var request = await _mediator.Send(command);

        return request.Match(
            response => Ok(response),
            errors => Problem(errors)
            );
    }
}