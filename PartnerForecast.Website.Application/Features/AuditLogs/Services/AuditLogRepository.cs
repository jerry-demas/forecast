using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore;
using PartnerForecast.Website.Application.Features.AuditLogs.Contracts;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Shared.Data;
using System.Linq.Expressions;
using System.Text.Json;

namespace PartnerForecast.Website.Application.Features.AuditLogs.Services;

public class AuditLogRepository(
    PartnerForecastDataContext dataContext
    )
    : IAuditLogRepository
{
    private readonly PartnerForecastDataContext _dataContext = dataContext;
       
    public async Task<Either<IEnumerable<AuditLogRecord>, PartnerForecastException>> GetAuditLogsByHoursId(int hoursId, CancellationToken cancellationToken)
        => await GetListAsync(
            al => al.ClientHoursId == hoursId, 
            cancellationToken);

    public async Task<Either<IEnumerable<AuditLogRecord>, PartnerForecastException>> GetAuditLogsByisEQRisNonbillable(GetLogsRequest request,  CancellationToken cancellationToken)
        => await GetListAsync(
            al => al.ClientHours.IsEQR == request.IsEqr && al.ClientHours.IsNonBillable == request.IsNonBillable && al.LogCategory == request.Category.ToString(),           
            cancellationToken);

    public async Task<Either<AuditLogRecord, PartnerForecastException>> AddAuditLog(AuditLogRecord log, CancellationToken cancellationToken)
         => await AddAsync(
                log,            
                cancellationToken);







    private async Task<Either<IEnumerable<AuditLogRecord>, PartnerForecastException>> GetListAsync(
       Expression<Func<AuditLogTable, bool>> predicate,
       CancellationToken cancellationToken)
    {

        try
        {
            var auditLogData = await _dataContext.AuditLogs
                .AsNoTracking()
                .Where(predicate)
                .Select(al => new
                {
                    al.Id,
                    al.ClientHoursId,
                    al.LogCategory,
                    al.ChangedByEmployeeNumber,
                    al.ChangedByEmployeeName,
                    al.ChangedByEmployeeDomain,
                    al.ChangeDescription,
                    al.Changes,
                    al.CreatedDateTime
                })
                .ToListAsync(cancellationToken);

            if (auditLogData == null || !auditLogData.Any()) {
                return new PartnerForecastException("No audit logs found.");
            }

            var result = auditLogData.Select(al => new AuditLogRecord
            (
                al.Id,
                al.ClientHoursId,
                al.LogCategory,
                al.ChangedByEmployeeNumber,
                al.ChangeDescription,
                al.Changes != null
                       ? JsonSerializer.Deserialize<IEnumerable<Change>>(al.Changes)
                       : null,
                al.CreatedDateTime,
                al.ChangedByEmployeeName,
                al.ChangedByEmployeeDomain
            )).ToList();

            return result;

        }
        catch (Exception ex) {
            return new PartnerForecastException(ex, ex.Message);
        }

    }



    private async Task<Either<AuditLogRecord, PartnerForecastException>> AddAsync(
        AuditLogRecord log,
        CancellationToken cancellationToken
    ) 
    {
        try
        {

            AuditLogTable newLog = log.ConvertToAuditLogTable();            
            var logEntity = await _dataContext.Set<AuditLogTable>().AddAsync(newLog, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return log;

        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex, ex.Message);
        }

    }
    



}
