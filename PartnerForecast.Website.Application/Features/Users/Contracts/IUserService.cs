using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.Users.Models;

namespace PartnerForecast.Website.Application.Features.Users.Contracts;


public  interface IUserService
{    
    Task<Either<User, PartnerForecastException>> GetCurrentUserByIdentityAsync(string? userIdentity, CancellationToken cancellationToken);
    Task<Either<List<User>, PartnerForecastException>> FindUsersByName(string name, CancellationToken cancellationToken);
    Task<Either<List<EqrUser>, PartnerForecastException>> GetNaoUsers(NaoUsersRequest request, CancellationToken cancellationToken);
    Task<Either<EqrUser, PartnerForecastException>> DeleteNaoUser(int userId, CancellationToken cancellationToken);
    Task<Either<EqrUser, PartnerForecastException>> UpdateNaoUser(EqrUser user, CancellationToken cancellationToken);
    Task<Either<EqrUser, PartnerForecastException>> AddNaoUser(EqrUser user, CancellationToken cancellationToken);
}

