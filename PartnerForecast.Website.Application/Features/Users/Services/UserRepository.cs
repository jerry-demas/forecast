using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore;
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

    /*
    public async Task<Either<EqrUser, PartnerForecastException>> GetCurrentUserAsync(
        int employeeNumber, 
        string userDomain, 
        CancellationToken cancellationToken)
        => await GetNaoUserInternalAsync(
            x => x.EmployeeNumber == employeeNumber &&
                 x.EmployeeDomain == userDomain &&
                 !x.IsInactive,                                          
            cancellationToken);
              
    

    public async Task<Either<List<EqrUser>, PartnerForecastException>> GetNaoUsers(NaoUsersRequest request,CancellationToken cancellationToken)
        => await GetNaoUsersInternalAsync(
            request, 
            cancellationToken
        );
    

    public async Task<Either<EqrUser, PartnerForecastException>> DeleteNaoUser(int userId, CancellationToken cancellationToken)              
        => await UpdateAsyncOrig(
            x => x.Id == userId,
            x => x.IsInactive = true,
            cancellationToken);       

    
    private async Task<Either<EqrUser, PartnerForecastException>> UpdateAsyncOrig(
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

            await _dataContext.SaveChangesAsync(cancellationToken);

            return user;
        }
        catch (Exception ex)
        {
        
            return new PartnerForecastException($"Error occurred while updating EQR user: {ex.Message}");
        }
    }
    
    private async Task<Either<List<EqrUser>, PartnerForecastException>> GetNaoUsersInternalAsync(
        Expression<Func<EqrUser, bool>>? predicate,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _dataContext.Set<EqrUser>().AsQueryable();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            var eqrUsers = await query.ToListAsync(cancellationToken);

            return eqrUsers;
        }
        catch (Exception ex)
        {
            return new PartnerForecastException($"Error occurred while retrieving EQR users: {ex.Message}");
        }
    }

    private async Task<Either<EqrUser, PartnerForecastException>> GetNaoUserInternalAsync(
        Expression<Func<EqrUser, bool>> predicate,
        CancellationToken cancellationToken)
    {
        try
        {           
            var user =  await _dataContext.Set<EqrUser>()
                .FirstOrDefaultAsync(
                    predicate, 
                    cancellationToken
                ); 
            if (user is null)
            {
                return new PartnerForecastException(
                    "User was not found.");
            }

            return user;
                   
        }
        catch (Exception ex)
        {
            return new PartnerForecastException($"Error occurred while retrieving EQR users: {ex.Message}");
        }
    }


    private async Task<Either<List<EqrUser>, PartnerForecastException>> GetNaoUsersInternalAsync(
        NaoUsersRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _dataContext.Set<EqrUser>().AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x => x.EmployeeName.Contains(request.SearchText) || x.EmployeeNumber.ToString().Contains(request.SearchText));
            }

            if (request.AdminOnly)
            {
                query = query.Where(x => x.IsAdmin);
            }

            if (request.ActiveOnly)
            {
                query = query.Where(x => !x.IsInactive);
            }

            var eqrUsers = await query.ToListAsync(cancellationToken);

            return eqrUsers;
        }
        catch (Exception ex)
        {
            return new PartnerForecastException($"Error occurred while retrieving EQR users: {ex.Message}");
        }
    }

    private async Task<TEntity?> GetAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) 
        where TEntity : class
    {        
        return await _dataContext.Set<TEntity>()
            .FirstOrDefaultAsync(
                predicate, 
                cancellationToken
            );        
    }
    */
    ///////////////////
    
    public async Task<Either<EqrUser, PartnerForecastException>> UpdateAsync(
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

            await _dataContext.SaveChangesAsync(cancellationToken);

            return user;
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
