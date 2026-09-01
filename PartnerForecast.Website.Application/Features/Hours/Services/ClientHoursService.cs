using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.Hours.Contracts;
using PartnerForecast.Website.Application.Features.Hours.Models;

namespace PartnerForecast.Website.Application.Features.Hours.Services;

public class ClientHoursService(IClientHoursRepository clientHoursRepository) : IClientHoursService
{
    private readonly IClientHoursRepository _clientHoursRepository = clientHoursRepository;

    public Task<Either<ClientHoursPagedResponse, PartnerForecastException>> GetClientHours(ClientHoursPagedRequest request, CancellationToken cancellationToken)                      
        => _clientHoursRepository.GetClientHours(request, cancellationToken);
       
    public Task<Either<ClientHours, PartnerForecastException>> GetClientHoursById(int id, CancellationToken cancellationToken)
        => _clientHoursRepository.GetClientHoursById(id, cancellationToken);

    public Task<Either<ClientHours, PartnerForecastException>> AddHour(ClientHours hours, CancellationToken cancellationToken)
        => _clientHoursRepository.AddHour(hours, cancellationToken);

    public Task<Either<ClientHours, PartnerForecastException>> DeleteHours(int id, CancellationToken cancellationToken)
        => _clientHoursRepository.DeleteHours(id, cancellationToken);

    public Task<Either<ClientHours, PartnerForecastException>> UpdateHours(ClientHours hours, CancellationToken cancellationToken)
        => _clientHoursRepository.UpdateHours(hours, cancellationToken);
}
