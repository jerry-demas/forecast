
using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.Clients.Contracts;
using PartnerForecast.Website.Application.Features.Clients.Models;

namespace PartnerForecast.Website.Application.Features.Clients.Services;

public class ClientService(
    IClientRepository clientRepository      
) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    
    public async Task<Either<IEnumerable<Client>, PartnerForecastException>> GetClientsContains(string value, CancellationToken cancellationToken)
        => await _clientRepository.GetClientsContains(value, cancellationToken);    

}
