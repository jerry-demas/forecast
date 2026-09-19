using Cbiz.SharedPackages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerForecast.Website.Application;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.Users.Contracts;
using PartnerForecast.Website.Application.Features.Users.Models;


namespace PartnerForecast.Website.Presentation.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController(
        IUserService _userservice
    ) : ControllerBase
{

    [HttpGet("CurrentUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CurrentUser(CancellationToken cancellationToken)
    {
        var userName = HttpContext?.User?.Identity?.Name;
        return (await _userservice.GetCurrentUserByIdentityAsync(userName, cancellationToken))
                .Match<IActionResult>(user => Ok(user),
                    (_, _failures) => NotFound(_failures.Message));
    }


    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> searchUsers([FromQuery] string name, CancellationToken cancellationToken)/// <param name="name">Search query string to match against user name</param>
    {
        return (await _userservice.FindUsersByName(name, cancellationToken))
                .Match<IActionResult>(users => Ok(users),
                    (_, _failures) => NotFound(_failures.Message));

    }


    [HttpGet("naoUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> naoUsers([FromQuery] NaoUsersRequest request, CancellationToken cancellationToken)/// <param name="name">Search query string to match against user name</param>
    {
        return (await _userservice.GetNaoUsers(request, cancellationToken))
                .Match<IActionResult>(users => Ok(users),
                    (_, _failures) => NotFound(_failures.Message));

    }

    [HttpDelete("naoUsers/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNaoUser(int userId, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser();
        if (currentUser.HasFailure)
        {
            return BadRequest(currentUser.Failure.Message);
        }
        return (await _userservice.DeleteNaoUser(userId, currentUser.Value, cancellationToken))
                .Match<IActionResult>(user => Ok(user),
                    (_, _failures) => NotFound(_failures.Message));
    }

    [HttpPut("naoUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateNaoUser([FromBody] EqrUser user, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser();
        if (currentUser.HasFailure)
        {
            return BadRequest(currentUser.Failure.Message);
        }
        return (await _userservice.UpdateNaoUser(user, currentUser.Value, cancellationToken))
                .Match<IActionResult>(user => Ok(user),
                    (_, _failures) => NotFound(_failures.Message));
    }



    [HttpPost("naoUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> AddNaoUser([FromBody] EqrUser user, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser();
        if (currentUser.HasFailure)
        {
            return BadRequest(currentUser.Failure.Message);
        }
        return (await _userservice.AddNaoUser(user, currentUser.Value, cancellationToken))
                .Match<IActionResult>(user => Ok(user),
                    (_, _failures) => NotFound(_failures.Message));
    }


    private async Task<Either<User, PartnerForecastException>> CurrentUser()
    {
        var userName = HttpContext?.User?.Identity?.Name;
        return await _userservice.GetCurrentUserByIdentityAsync(userName, CancellationToken.None);
    }
}
