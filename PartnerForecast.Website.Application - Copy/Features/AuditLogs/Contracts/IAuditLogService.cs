using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;


namespace PartnerForecast.Website.Application.Features.AuditLogs.Contracts;

public interface IAuditLogService
{
    Task<Either<IEnumerable<AuditLogRecord>, PartnerForecastException>> GetAuditLogs(GetLogsRequest request, CancellationToken cancellationToken);   
    Task<Either<AuditLogRecord, PartnerForecastException>> AddAuditLog(AuditLogRecord log, CancellationToken cancellationToken);    
}
