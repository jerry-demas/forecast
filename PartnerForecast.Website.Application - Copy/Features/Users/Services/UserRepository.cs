using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.Users.Contracts;
using PartnerForecast.Website.Application.Features.Users.Models;
using PartnerForecast.Website.Application.Shared.Data;
using System.Linq.Expressions;

namespace PartnerForecast.Website.Application.Features.Users.Services;

public class UserRepository (
    PartnerForecastDataContext dataContext,
    ILogger<UserRepository> logger
    ) : IUserRepository
{
    private readonly PartnerForecastDataContext _dataContext = dataContext;
    private readonly ILogger<UserRepository> _logger = logger;
    
    public async Task<Either<UserUpdate, PartnerForecastException>> UpdateAsync(
        Expression<Func<EqrUser, bool>> predicate,
        Action<EqrUser> updateAction,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating NAO user with predicate: {Predicate}", predicate);
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
            _logger.LogError(ex, "Error occurred while updating NAO user with predicate: {Predicate}", predicate);
            return new PartnerForecastException($"Error occurred while updating EQR user: {ex.Message}");
        }
    } 
    

    public async Task<Either<EqrUser, PartnerForecastException>> GetSingleAsync(
        Expression<Func<EqrUser, bool>> predicate,        
        CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving NAO user with predicate: {Predicate}", predicate);
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
                _logger.LogError(ex, "Error occurred while retrieving EQR user with predicate: {Predicate}", predicate);
                return new PartnerForecastException($"Error occurred while retrieving EQR user: {ex.Message}");
            }
        }


    public async Task<Either<List<EqrUser>, PartnerForecastException>> GetListAsync(
        Expression<Func<EqrUser, bool>> predicate,        
        CancellationToken cancellationToken)    
        {
            _logger.LogInformation("Retrieving NAO user list with predicate: {Predicate}", predicate);
            try
            {
                var users = await _dataContext.Set<EqrUser>()
                    .Where(predicate)
                    .ToListAsync(cancellationToken);

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving NAO user list with predicate: {Predicate}", predicate);
                return new PartnerForecastException($"Error occurred while retrieving EQR users: {ex.Message}");
            }
        }

    public async Task<Either<EqrUser, PartnerForecastException>> AddAsync(
        EqrUser user, 
        CancellationToken cancellationToken)
    {       
         _logger.LogInformation("Adding NAO user: {User}", user);
        try
        {
            _dataContext.Set<EqrUser>().Add(user);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding NAO user: {User}", user);
            return new PartnerForecastException(ex, ex.Message);
        }
    }
    
}
