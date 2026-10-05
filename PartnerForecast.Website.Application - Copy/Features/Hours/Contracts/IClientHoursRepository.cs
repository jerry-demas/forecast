using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.Clients.Models;
using PartnerForecast.Website.Application.Features.Hours.Models;
using System.Linq.Expressions;

namespace PartnerForecast.Website.Application.Features.Hours.Contracts;

public interface IClientHoursRepository
{   
    Task<Either<IEnumerable<ClientHours>, PartnerForecastException>> GetListAsync(
        Expression<Func<ClientHours, bool>> predicate,       
        CancellationToken cancellationToken);

    Task<Either<ClientHoursUpdate, PartnerForecastException>> UpdateAsync(
        Expression<Func<ClientHours, bool>> predicate,
        Action<ClientHours> updateAction,
        CancellationToken cancellationToken);

    Task<Either<ClientHours, PartnerForecastException>> GetSingleAsync(
        Expression<Func<ClientHours, bool>> predicate,       
        CancellationToken cancellationToken);

    Task<Either<ClientHours, PartnerForecastException>> AddAsync(
        ClientHours clientHours, 
        CancellationToken cancellationToken);
    
     Task<Either<ClientHoursPagedResponse, PartnerForecastException>> GetListPagedAsync(      
       ClientHoursPagedRequest pagedRequest,
       CancellationToken cancellationToken);

}
