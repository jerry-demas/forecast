using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.Clients.Models;

namespace PartnerForecast.Website.Application.Features.Clients.Contracts;

public interface IClientRepository
{   
    Task<Either<IEnumerable<Client>, PartnerForecastException>> GetClientsContains(string value, CancellationToken cancellationToken);
}
