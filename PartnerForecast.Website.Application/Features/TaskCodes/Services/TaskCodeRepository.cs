using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Contracts;
using PartnerForecast.Website.Application.Features.TaskCodes.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;
using PartnerForecast.Website.Application.Shared.Data;
using System.Linq.Expressions;

namespace PartnerForecast.Website.Application.Features.TaskCodes.Services;

public class TaskCodeRepository(
    PartnerForecastDataContext dataContext
) : ITaskCodeRepository
{
    private readonly PartnerForecastDataContext _dataContext = dataContext;

    public async Task<Either<IEnumerable<TaskCode>, PartnerForecastException>> GetListAsync(
        Expression<Func<TaskCode, bool>> predicate, 
        CancellationToken cancellationToken)
    {
        try
            {
                var codes = await _dataContext.Set<TaskCode>()
                    .Where(predicate)
                    .ToListAsync(cancellationToken);

                return codes;
            }
            catch (Exception ex)
            {
                return new PartnerForecastException($"Error occurred while retrieving Task codes: {ex.Message}");
            }
    }

    public async Task<Either<TaskCodeUpdate, PartnerForecastException>> UpdateAsync(
        Expression<Func<TaskCode, bool>> predicate, 
        Action<TaskCode> updateAction, 
        CancellationToken cancellationToken)
    {
        try
        {
            var taskCode = await _dataContext.Set<TaskCode>()
                .FirstOrDefaultAsync(predicate, cancellationToken);

            if (taskCode is null)
            {
                return new PartnerForecastException("Task code not found.");
            }

            updateAction(taskCode);

            var changes = _dataContext.Entry(taskCode)
                .Properties
                .Where(p => p.IsModified)
                .Select(p => new Change(                
                    p.Metadata.Name,
                    p.OriginalValue?.ToString() ?? string.Empty,
                    p.CurrentValue?.ToString() ?? string.Empty
                ))
                .ToArray();


            await _dataContext.SaveChangesAsync(cancellationToken);

            return new TaskCodeUpdate(taskCode, changes);
        }
        catch (Exception ex)
        {
        
            return new PartnerForecastException($"Error occurred while updating Task code: {ex.Message}");
        }
    }

    public async Task<Either<TaskCode, PartnerForecastException>> GetSingleAsync(
        Expression<Func<TaskCode, bool>> predicate, 
        CancellationToken cancellationToken)
    {
         try
            {
                var code = await _dataContext.Set<TaskCode>()
                    .FirstOrDefaultAsync(predicate, cancellationToken);

                if (code is null)
                {
                    return new PartnerForecastException("Task code not found.");
                }

                return code;
            }
            catch (Exception ex)
            {
                return new PartnerForecastException($"Error occurred while retrieving Task code: {ex.Message}");
            }
    }

    public async Task<Either<TaskCode, PartnerForecastException>> AddAsync(
        TaskCode taskCode, 
        CancellationToken cancellationToken)
    {
         try
        {
            _dataContext.Set<TaskCode>().Add(taskCode);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return taskCode;
        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex, ex.Message);
        }
    }
}