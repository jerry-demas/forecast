using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.Users.Models;

namespace PartnerForecast.Website.Application.Features.Users.Contracts;

public interface IPartnerForecastLdapService
{
    Task<Either<LdapUser, PartnerForecastException>> GetCurrentUserByIdentityAsync(string userIdentity, CancellationToken cancellationToken);
    Task<Either<List<LdapUser>, PartnerForecastException>> FindUsersByName(string userName, CancellationToken cancellationToken);
}
