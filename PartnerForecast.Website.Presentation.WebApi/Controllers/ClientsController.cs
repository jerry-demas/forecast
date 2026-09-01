using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerForecast.Website.Application.Features.Clients.Contracts;

namespace PartnerForecast.Website.Presentation.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class ClientsController(
    IClientService _clientService
) : ControllerBase
{

    [HttpGet("{clientName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ClientContains(string clientName, CancellationToken cancellationToken)
    {
        return (await _clientService.GetClientsContains(clientName, cancellationToken))
                .Match<IActionResult>(clients => Ok(clients),
                    (_, _failures) => NotFound(_failures.Message));
    }
}
