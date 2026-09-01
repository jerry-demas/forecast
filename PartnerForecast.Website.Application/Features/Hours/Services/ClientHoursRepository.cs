using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore;
using PartnerForecast.Website.Application.Features.Hours.Contracts;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Shared.Data;
using System.Linq.Expressions;


namespace PartnerForecast.Website.Application.Features.Hours.Services;

public class ClientHoursRepository(
    PartnerForecastDataContext dataContext
    ) : IClientHoursRepository
{
    private readonly PartnerForecastDataContext _dataContext = dataContext;

    public async Task<Either<ClientHoursPagedResponse, PartnerForecastException>> GetClientHours(ClientHoursPagedRequest request, CancellationToken cancellationToken)
        => await GetListInternalPagedAsync(                            
            request,            
            cancellationToken);
    
    public async Task<Either<ClientHours, PartnerForecastException>> GetClientHoursById(int id, CancellationToken cancellationToken)
        => await GetSingleInternalAsync(
            x => x.Id == id, 
            cancellationToken);
       
    public async Task<Either<ClientHours, PartnerForecastException>> AddHour(ClientHours hours, CancellationToken cancellationToken)
        => await AddHourInternalAsync(
            hours,
            cancellationToken);

    public async Task<Either<ClientHours, PartnerForecastException>> DeleteHours(int id, CancellationToken cancellationToken)
        => await UpdateAsync(
            x => x.Id == id,
            x => x.IsDeleted = true,    
            cancellationToken);

    public async Task<Either<ClientHours, PartnerForecastException>> UpdateHours(ClientHours hours, CancellationToken cancellationToken)
        => await UpdateAsync(
            hours,
            cancellationToken);



    



    private async Task<Either<ClientHours, PartnerForecastException>> AddHourInternalAsync(
        ClientHours hours,
        CancellationToken cancellationToken)
    {
        try
        {
            _dataContext.Set<ClientHours>().Add(hours);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return hours;
        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex, ex.Message);
        }
    }

    private async Task<Either<ClientHours, PartnerForecastException>> GetSingleInternalAsync(
        Expression<Func<ClientHours, bool>>? predicate,
        CancellationToken cancellationToken)
    {

        try
        {
            var query = _dataContext.Set<ClientHours>()
               .AsNoTracking();

            if (predicate != null) 
            { 
                query = query.Where(predicate); 
            }

            var hours = await query.FirstOrDefaultAsync(cancellationToken);

            if (hours is null)
            {
                return new PartnerForecastException(
                    "Client hours were not found.");
            }

            return hours;
        }
        catch (Exception ex) {
            return new PartnerForecastException(ex, ex.Message);
        }

    }

    
    private async Task<Either<ClientHoursPagedResponse, PartnerForecastException>> GetListInternalPagedAsync(      
       ClientHoursPagedRequest pagedRequest,
       CancellationToken cancellationToken)
    {
        try
        {

            IQueryable<ClientHours> query = _dataContext.Set<ClientHours>();

            if (pagedRequest.hourRequest != null)
            {
                query = query.Where(_ => _.IsEQR== pagedRequest.hourRequest.isEqr &&
                                         _.IsNonBillable == pagedRequest.hourRequest.isNonBillable &&
                                         !_.IsDeleted);

                if (pagedRequest.hourRequest.EmployeeNumberAssigned > 0)
                {
                    query = query.Where(_ =>
                        _.EmployeeNumberAssigned ==
                        pagedRequest.hourRequest.EmployeeNumberAssigned);
                }

                if (!string.IsNullOrWhiteSpace(pagedRequest.hourRequest.CustomerNumber))
                {
                    query = query.Where(_ =>
                        _.CustomerNumber ==
                        pagedRequest.hourRequest.CustomerNumber);
                }

                if (pagedRequest.hourRequest.Year > 0)
                {
                    query = query.Where(_ =>
                        _.Year == pagedRequest.hourRequest.Year);
                }

                if (pagedRequest.hourRequest.Month > 0)
                {
                    query = query.Where(_ =>
                        _.Month == pagedRequest.hourRequest.Month);
                }
                
            }

            query = query
                .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
                .Take(pagedRequest.PageSize);

            if (pagedRequest.SortField is not null)
            {
                query = pagedRequest.SortDescending
                    ? query.OrderByDescending(e => EF.Property<object>(e, pagedRequest.SortField))
                    : query.OrderBy(e => EF.Property<object>(e, pagedRequest.SortField));
            }
            
            var results =  await query.ToListAsync(cancellationToken);
            return new ClientHoursPagedResponse(
                pagedRequest.PageNumber,
                pagedRequest.PageSize,
                results.Count,
                results);                                            
        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex, ex.Message);


        }
        

    }

    
    private async Task<Either<ClientHours, PartnerForecastException>> UpdateAsync(
        Expression<Func<ClientHours, bool>> predicate,
        Action<ClientHours> updateAction,
        CancellationToken cancellationToken)
       
    {
        try
        {
            
            var entity = await _dataContext.Set<ClientHours>().FirstOrDefaultAsync(predicate, cancellationToken);
            
            if (entity is null) return new PartnerForecastException("Client hours not found.");
                                    
            updateAction(entity);
            
            var records = await _dataContext.SaveChangesAsync(cancellationToken);
            
            _dataContext.ChangeTracker.Clear();
                       
            return entity;

        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex, ex.Message);
        }
    }


    private async Task<Either<ClientHours, PartnerForecastException>> UpdateAsync(
        ClientHours updatedHours,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dataContext.Set<ClientHours>()
                .FirstOrDefaultAsync(
                    x => x.Id == updatedHours.Id,
                    cancellationToken);

            if (entity is null)
                return new PartnerForecastException("Client hours not found.");

            _dataContext.Entry(entity)
                .CurrentValues
                .SetValues(updatedHours);

            await _dataContext.SaveChangesAsync(cancellationToken);

            _dataContext.ChangeTracker.Clear();

            return entity;
        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex, ex.Message);
        }
    }
}
