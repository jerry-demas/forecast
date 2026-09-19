using Cbiz.SharedPackages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerForecast.Website.Application;
using PartnerForecast.Website.Application.Features.TaskCodes.Contracts;
using PartnerForecast.Website.Application.Features.TaskCodes.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;
using PartnerForecast.Website.Application.Features.Users.Contracts;
using PartnerForecast.Website.Application.Features.Users.Models;

namespace PartnerForecast.Website.Presentation.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class TaskCodesController(
    ITaskCodeService _taskCodeService,
    IUserService _userservice
) : ControllerBase
{


    /// <summary>
    /// Retrieves task codes.
    /// </summary>
    /// <param name="request">
    /// The request containing search text and active-only filter.
    /// </param>
    /// <returns>A collection of task codes.</returns>
    /// <response code="200">Task codes were successfully retrieved.</response>
    /// <response code="404">Task codes were not found.</response>

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTaskCodes([FromQuery] TaskCodeRequest request, CancellationToken cancellationToken)
    {
        return (await _taskCodeService.GetTaskCodes(request, cancellationToken))
                .Match<IActionResult>(codes => Ok(codes),
                    (_, _failures) => NotFound(_failures.Message));
    }

    [HttpGet("Code/{taskCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Code(string taskCode, CancellationToken cancellationToken)
    {
        return (await _taskCodeService.GetTaskCodeByCode(taskCode, cancellationToken))
                .Match<IActionResult>(codes => Ok(codes),
                    (_, _failures) => NotFound(_failures.Message));
    }

    [HttpGet("{taskId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Id(int taskId, CancellationToken cancellationToken)
    {
        return (await _taskCodeService.GetTaskCodeById(taskId, cancellationToken))
                .Match<IActionResult>(codes => Ok(codes),
                    (_, _failures) => NotFound(_failures.Message));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] TaskCode taskCode, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser();
        if (currentUser.HasFailure)
        {
            return BadRequest(currentUser.Failure.Message);
        }
        return (await _taskCodeService.AddTaskCode(taskCode, currentUser.Value, cancellationToken))
                .Match<IActionResult>(code => Ok(code),
                    (_, _failures) => BadRequest(_failures.Message));
    }

    [HttpDelete("{taskId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int taskId, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser();
        if (currentUser.HasFailure)
        {
            return BadRequest(currentUser.Failure.Message);
        }
        return (await _taskCodeService.DeleteTaskCode(taskId, currentUser.Value, cancellationToken))
                .Match<IActionResult>(code => Ok(code),
                    (_, _failures) => BadRequest(_failures.Message));
    }


    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] TaskCode taskCode, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser();
        if (currentUser.HasFailure)
        {
            return BadRequest(currentUser.Failure.Message);
        }
        return (await _taskCodeService.UpdateTaskCode(taskCode, currentUser.Value, cancellationToken))
                .Match<IActionResult>(code => Ok(code),
                    (_, _failures) => BadRequest(_failures.Message));
    }

    
    private async Task<Either<User, PartnerForecastException>> CurrentUser()
    {
        var userName = HttpContext?.User?.Identity?.Name;
        return await _userservice.GetCurrentUserByIdentityAsync(userName, CancellationToken.None);
    }

}
