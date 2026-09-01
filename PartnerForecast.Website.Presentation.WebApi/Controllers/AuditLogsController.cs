using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerForecast.Website.Application.Features.AuditLogs.Contracts;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;

namespace PartnerForecast.Website.Presentation.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class AuditLogsController(
    IAuditLogService _auditLogService
) : ControllerBase
{

    [HttpGet("HoursId/{hoursId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> HoursId(int hoursId, CancellationToken cancellationToken)
    {
        return (await _auditLogService.GetAuditLogsByHoursId(hoursId, cancellationToken))
                .Match<IActionResult>(logs => Ok(logs),
                    (_, _failures) => NotFound(_failures.Message));
    }

        
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]    
    public async Task<IActionResult> GetLogsByIsEQRandIsNonBillable([FromQuery] GetLogsRequest request, CancellationToken cancellationToken)
    {
        return (await _auditLogService.GetAuditLogsByisEQRisNonbillable(request, cancellationToken))
                 .Match<IActionResult>(result => Ok(result),
                    (_, failures) => BadRequest(failures.Message));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] AuditLogRecord auditLog, CancellationToken cancellationToken)
    {
        return (await _auditLogService.AddAuditLog(auditLog, cancellationToken))
                .Match<IActionResult>(code => Ok(code),
                    (_, _failures) => BadRequest(_failures.Message));
    }


}
