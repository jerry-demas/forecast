using Azure;
using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.Clients.Models;
using PartnerForecast.Website.Application.Features.Hours.Contracts;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;
using PartnerForecast.Website.Application.Shared.Data;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;


namespace PartnerForecast.Website.Application.Features.Hours.Services;

public class ClientHoursRepository(
    PartnerForecastDataContext dataContext
    ) : IClientHoursRepository
{
    private readonly PartnerForecastDataContext _dataContext = dataContext;

     public async Task<Either<ClientHoursPagedResponse, PartnerForecastException>> GetListPagedAsync(      
       ClientHoursPagedRequest pagedRequest,
       CancellationToken cancellationToken)
    {
        try
        {
            IQueryable<ClientHours> query = _dataContext.Set<ClientHours>();

            if (pagedRequest.hourRequest is not null)
            {
                query = query.Where(x =>
                    x.IsEQR == pagedRequest.hourRequest.isEqr &&
                    x.IsNonBillable == pagedRequest.hourRequest.isNonBillable &&
                    !x.IsDeleted);

                if (!string.IsNullOrWhiteSpace(pagedRequest.hourRequest.SearchText))
                {
                    string searchText = pagedRequest.hourRequest.SearchText.Trim();

                    if (pagedRequest.hourRequest.isNonBillable)
                    {                                                
                        query = query.Where(x =>
                            x.TaskCode != null &&
                            x.TaskCode.Contains(searchText));                                                                           
                    }
                    else
                    {                       
                        query = query.Where(x =>
                            x.CustomerName.Contains(searchText) ||
                            x.CustomerNumber.Contains(searchText));                        
                    }
                }

                if(pagedRequest.hourRequest.ClientNumber is not null)
                {
                    query = query.Where(x =>
                        x.CustomerNumber == pagedRequest.hourRequest.ClientNumber);
                }

                if(!string.IsNullOrWhiteSpace(pagedRequest.hourRequest.TaskCode))
                {
                    query = query.Where(x =>
                        x.TaskCode == pagedRequest.hourRequest.TaskCode);
                }

                if (pagedRequest.hourRequest.EmployeeNumber > 0)
                {
                    query = query.Where(x =>
                        x.EmployeeNumber == pagedRequest.hourRequest.EmployeeNumber);
                }

                if (pagedRequest.hourRequest.forNewHours)
                {                                     
                    var startDate = new DateTime(2026, 9, 1);
                    var endDate = startDate.AddMonths(12);

                    var startYearMonth = startDate.Year * 100 + startDate.Month;
                    var endYearMonth = endDate.Year * 100 + endDate.Month;

                    query = query.Where(x =>
                        (x.Year * 100 + x.Month) >= startYearMonth &&
                        (x.Year * 100 + x.Month) <= endYearMonth);

                } else {

                    if (pagedRequest.hourRequest.Year > 0)
                    {
                        query = query.Where(x =>
                            x.Year == pagedRequest.hourRequest.Year);
                    }

                    if (pagedRequest.hourRequest.Month > 0)
                    {
                        query = query.Where(x =>
                            x.Month == pagedRequest.hourRequest.Month);
                    }
                }



            }

            int totalCount = await query.CountAsync(cancellationToken);

            var resultQuery =
                from clientHours in query
                join taskCode in _dataContext.Set<TaskCode>()
                    on clientHours.TaskCode equals taskCode.Code into taskCodes
                from taskCode in taskCodes.DefaultIfEmpty()

                select new ClientHoursResponse
                {
                    Id = clientHours.Id,
                    EmployeeNumber = clientHours.EmployeeNumber,
                    EmployeeNameAssigned = clientHours.EmployeeNameAssigned,
                    EmployeeDomainAssigned = clientHours.EmployeeDomainAssigned,
                    CustomerName = clientHours.CustomerName,
                    CustomerNumber = clientHours.CustomerNumber,
                    Month = clientHours.Month,
                    Year = clientHours.Year,
                    Hours = clientHours.Hours,
                    TaskCode = clientHours.TaskCode,
                    
                    TaskCodeDescription = taskCode == null
                        ? null
                        : taskCode.CodeDescription,
                                    
                    IsEQR = clientHours.IsEQR,
                    IsNonBillable = clientHours.IsNonBillable,
                    EmployeeNumberAssigned = clientHours.EmployeeNumberAssigned
                };

            if (!string.IsNullOrWhiteSpace(pagedRequest.SortField))
            {
                resultQuery = pagedRequest.SortDescending
                    ? resultQuery.OrderByDescending(x =>
                        EF.Property<object>(x, pagedRequest.SortField))
                    : resultQuery.OrderBy(x =>
                        EF.Property<object>(x, pagedRequest.SortField));
            }
            else
            {
                resultQuery = resultQuery.OrderBy(x => x.Id);
            }          
            var results = await resultQuery
                .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
                .Take(pagedRequest.PageSize)
                .ToListAsync(cancellationToken);

            return new ClientHoursPagedResponse(
                pagedRequest.PageNumber,
                pagedRequest.PageSize,
                totalCount,
                results);
        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex, ex.Message);
        }        

    }

    public async Task<Either<IEnumerable<ClientHours>, PartnerForecastException>> GetListAsync(
        Expression<Func<ClientHours, bool>> predicate, 
        CancellationToken cancellationToken)
    {
         try
            {
                var hours = await _dataContext.Set<ClientHours>()
                    .Where(predicate)
                    .ToListAsync(cancellationToken);

                return hours;
            }
            catch (Exception ex)
            {
                return new PartnerForecastException($"Error occurred while retrieving Task codes: {ex.Message}");
            }
    }

    public async Task<Either<ClientHoursUpdate, PartnerForecastException>> UpdateAsync(
        Expression<Func<ClientHours, bool>> predicate, 
        Action<ClientHours> updateAction, 
        CancellationToken cancellationToken)
    {
       try
        {
            var clientHours = await _dataContext.Set<ClientHours>()
                .FirstOrDefaultAsync(predicate, cancellationToken);

            if (clientHours is null)
            {
                return new PartnerForecastException("Client hours not found.");
            }

            updateAction(clientHours);

            var changes = _dataContext.Entry(clientHours)
                .Properties
                .Where(p => p.IsModified &&
                    p.Metadata.Name != nameof(ClientHours.LastUpdatedDateTime))
                .Select(p => new Change(                
                    p.Metadata.Name,
                    p.OriginalValue?.ToString() ?? string.Empty,
                    p.CurrentValue?.ToString() ?? string.Empty
                ))
                .ToArray();

            await _dataContext.SaveChangesAsync(cancellationToken);

            return new ClientHoursUpdate(clientHours, changes);
        }
        catch (Exception ex)
        {
        
            return new PartnerForecastException($"Error occurred while updating Client hours: {ex.Message}");
        }
    }

    public async Task<Either<ClientHours, PartnerForecastException>> GetSingleAsync(
        Expression<Func<ClientHours, bool>> predicate, 
        CancellationToken cancellationToken)
    {
         try
            {
                var clientHours = await _dataContext.Set<ClientHours>()
                    .FirstOrDefaultAsync(predicate, cancellationToken);

                if (clientHours  is null)
                {
                    return new PartnerForecastException("Client hours not found.");
                }

                return clientHours;
            }
            catch (Exception ex)
            {
                return new PartnerForecastException($"Error occurred while retrieving Client hours: {ex.Message}");
            }
    }

    public async Task<Either<ClientHours, PartnerForecastException>> AddAsync(
        ClientHours clientHours, 
        CancellationToken cancellationToken)
    {
        try
        {
            _dataContext.Set<ClientHours>().Add(clientHours);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return clientHours;
        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex, ex.Message);
        }
    }
}
