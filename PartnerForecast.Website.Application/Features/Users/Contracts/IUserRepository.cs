using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.Users.Models;
using System.Linq.Expressions;


namespace PartnerForecast.Website.Application.Features.Users.Contracts;

public interface IUserRepository
{
        Task<Either<EqrUser, PartnerForecastException>> UpdateAsync(
        Expression<Func<EqrUser, bool>> predicate,
        Action<EqrUser> updateAction,
        CancellationToken cancellationToken);

    Task<Either<EqrUser, PartnerForecastException>> GetSingleAsync(
        Expression<Func<EqrUser, bool>> predicate,       
        CancellationToken cancellationToken);

    Task<Either<List<EqrUser>, PartnerForecastException>> GetListAsync(
        Expression<Func<EqrUser, bool>> predicate,       
        CancellationToken cancellationToken);

    Task<Either<EqrUser, PartnerForecastException>> AddAsync(
        EqrUser EqrUser, 
        CancellationToken cancellationToken);

}
