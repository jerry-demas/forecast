using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;
using System.Linq.Expressions;


namespace PartnerForecast.Website.Application.Features.TaskCodes.Contracts;

public interface ITaskCodeRepository
{
    
    Task<Either<IEnumerable<TaskCode>, PartnerForecastException>> GetListAsync(
        Expression<Func<TaskCode, bool>> predicate,       
        CancellationToken cancellationToken);

    Task<Either<TaskCode, PartnerForecastException>> UpdateAsync(
        Expression<Func<TaskCode, bool>> predicate,
        Action<TaskCode> updateAction,
        CancellationToken cancellationToken);

    Task<Either<TaskCode, PartnerForecastException>> GetSingleAsync(
        Expression<Func<TaskCode, bool>> predicate,       
        CancellationToken cancellationToken);

    Task<Either<TaskCode, PartnerForecastException>> AddAsync(
        TaskCode taskCode, 
        CancellationToken cancellationToken);
}
