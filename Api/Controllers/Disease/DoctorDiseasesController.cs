using Application.Diseases.Commands.AddDisease;
using Application.Diseases.Commands.DeleteDisease;
using Application.Diseases.Commands.UpdateDisease;
using Application.Diseases.Queries.GetPatientDiseases;
using Core.Account.enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Disease;

[Route("disease")]
[Authorize(Roles = nameof(UserRoles.Doctor))]
public class DoctorDiseasesController : ApiController
{
    private readonly IMediator _mediator;

    public DoctorDiseasesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Set new disease
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SetDisease([FromBody] AddDiseaseCommand command)
    {
        var response = await _mediator.Send(command);

        return response.Match(
            diseaseResponse => Ok(diseaseResponse),
            errors => Problem(errors)
            );
    }

    /// <summary>
    /// Update mistakes in disease
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> UpdateDisease([FromBody] UpdateDiseaseCommand command)
    {
        var response = await _mediator.Send(command);
        
        return response.Match(
            diseaseResponse => Ok(diseaseResponse),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Delete disease
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteDisease([FromBody] DeleteDiseaseCommand command)
    {
        var response = await _mediator.Send(command);
        
        return response.Match(
            diseaseResponse => Ok(diseaseResponse),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Display all diseases of your patient
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("{PatientId}")]
    public async Task<IActionResult> GetPatientDiseases([FromRoute] GetPatientDiseasesQuery query)
    {
        var response = await _mediator.Send(query);

        return response.Match(
            diseasesResponse => Ok(diseasesResponse),
            errors => Problem(errors)
        );
    }
}