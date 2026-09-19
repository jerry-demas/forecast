using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.Users.Contracts;
using PartnerForecast.Website.Application.Features.Users.Models;
using PartnerForecast.Website.Application.Shared.Data;
using System.Linq.Expressions;

namespace PartnerForecast.Website.Application.Features.Users.Services;

public class UserRepository (
    PartnerForecastDataContext dataContext
    ) : IUserRepository
{
    private readonly PartnerForecastDataContext _dataContext = dataContext;
    
    public async Task<Either<UserUpdate, PartnerForecastException>> UpdateAsync(
        Expression<Func<EqrUser, bool>> predicate,
        Action<EqrUser> updateAction,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _dataContext.Set<EqrUser>()
                .FirstOrDefaultAsync(predicate, cancellationToken);

            if (user is null)
            {
                return new PartnerForecastException("User not found.");
            }

            updateAction(user);

            var changes = _dataContext.Entry(user)
                .Properties
                .Where(p => p.IsModified)
                .Select(p => new Change(                
                    p.Metadata.Name,
                    p.OriginalValue?.ToString() ?? string.Empty,
                    p.CurrentValue?.ToString() ?? string.Empty
                ))
                .ToArray();



            await _dataContext.SaveChangesAsync(cancellationToken);

            return new UserUpdate(user, changes);
        }
        catch (Exception ex)
        {
        
            return new PartnerForecastException($"Error occurred while updating EQR user: {ex.Message}");
        }
    } 
    

    public async Task<Either<EqrUser, PartnerForecastException>> GetSingleAsync(
        Expression<Func<EqrUser, bool>> predicate,        
        CancellationToken cancellationToken)
        {
            try
            {
                var user = await _dataContext.Set<EqrUser>()
                    .FirstOrDefaultAsync(predicate, cancellationToken);

                if (user is null)
                {
                    return new PartnerForecastException("User not found.");
                }

                return user;
            }
            catch (Exception ex)
            {
                return new PartnerForecastException($"Error occurred while retrieving EQR user: {ex.Message}");
            }
        }


    public async Task<Either<List<EqrUser>, PartnerForecastException>> GetListAsync(
        Expression<Func<EqrUser, bool>> predicate,        
        CancellationToken cancellationToken)    
        {
            try
            {
                var users = await _dataContext.Set<EqrUser>()
                    .Where(predicate)
                    .ToListAsync(cancellationToken);

                return users;
            }
            catch (Exception ex)
            {
                return new PartnerForecastException($"Error occurred while retrieving EQR users: {ex.Message}");
            }
        }

    public async Task<Either<EqrUser, PartnerForecastException>> AddAsync(
        EqrUser user, 
        CancellationToken cancellationToken)
    {       
        try
        {
            _dataContext.Set<EqrUser>().Add(user);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return user;
        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex, ex.Message);
        }
    }
    
}
