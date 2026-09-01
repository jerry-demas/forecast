using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerForecast.Website.Application.Features.Hours.Contracts;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;

namespace PartnerForecast.Website.Presentation.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ClientHoursController(
    IClientHoursService _clientHoursService
    ) : ControllerBase
{


    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHours([FromQuery] ClientHoursPagedRequest request, CancellationToken cancellationToken)
    {

        return (await _clientHoursService.GetClientHours(request, cancellationToken))
                .Match<IActionResult>(hours => Ok(hours),
                    (_, _failures) => NotFound(_failures.Message));

    }

    [HttpGet("HoursId/{hoursId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHoursById(int hoursId, CancellationToken cancellationToken)
    {

        return (await _clientHoursService.GetClientHoursById(hoursId, cancellationToken))
                .Match<IActionResult>(hours => Ok(hours),
                    (_, _failures) => NotFound(_failures.Message));

    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] ClientHours clientHours, CancellationToken cancellationToken)
    {
        return (await _clientHoursService.AddHour(clientHours, cancellationToken))
                .Match<IActionResult>(hour => Ok(hour),
                    (_, _failures) => BadRequest(_failures.Message));
    }

    [HttpDelete("{hoursId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int hoursId, CancellationToken cancellationToken)
    {
        return (await _clientHoursService.DeleteHours(hoursId, cancellationToken))
                .Match<IActionResult>(hour => Ok(hour),
                    (_, _failures) => BadRequest(_failures.Message));
    }

    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] ClientHours clientHours, CancellationToken cancellationToken)
    {
        return (await _clientHoursService.UpdateHours(clientHours, cancellationToken))
                .Match<IActionResult>(hour => Ok(hour),
                    (_, _failures) => BadRequest(_failures.Message));
    }

}
