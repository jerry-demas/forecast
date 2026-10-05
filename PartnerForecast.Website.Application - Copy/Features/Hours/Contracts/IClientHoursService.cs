using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.Users.Models;


namespace PartnerForecast.Website.Application.Features.Hours.Contracts;

public interface IClientHoursService
{
    Task<Either<ClientHoursPagedResponse, PartnerForecastException>> GetClientHours(ClientHoursPagedRequest request, CancellationToken cancellationToken);
    Task<Either<ClientHours, PartnerForecastException>> GetClientHoursById(int id, CancellationToken cancellationToken);
    Task<Either<ClientHours, PartnerForecastException>> AddHour(ClientHours hours, User currentUser, CancellationToken cancellationToken);
    Task<Either<ClientHours, PartnerForecastException>> DeleteHours(int id, User currentUser, CancellationToken cancellationToken);
    Task<Either<ClientHours, PartnerForecastException>> UpdateHours(ClientHours hours, User currentUser, CancellationToken cancellationToken);
}
