using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;

namespace PartnerForecast.Website.Application.Features.Hours.Contracts;

public interface IClientHoursRepository
{
    Task<Either<ClientHoursPagedResponse, PartnerForecastException>> GetClientHours(ClientHoursPagedRequest request, CancellationToken cancellationToken);
    Task<Either<ClientHours, PartnerForecastException>> GetClientHoursById(int id, CancellationToken cancellationToken);
    Task<Either<ClientHours, PartnerForecastException>> AddHour(ClientHours hours, CancellationToken cancellationToken);
    Task<Either<ClientHours, PartnerForecastException>> DeleteHours(int id, CancellationToken cancellationToken);
    Task<Either<ClientHours, PartnerForecastException>> UpdateHours(ClientHours hours, CancellationToken cancellationToken);
}
