using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;


namespace PartnerForecast.Website.Application.Features.AuditLogs.Contracts;

public interface IAuditLogService
{
    Task<Either<IEnumerable<AuditLogRecord>, PartnerForecastException>> GetAuditLogsByisEQRisNonbillable(GetLogsRequest request, CancellationToken cancellationToken);
    Task<Either<IEnumerable<AuditLogRecord>, PartnerForecastException>> GetAuditLogsByHoursId(int hoursId, CancellationToken cancellationToken);
    Task<Either<AuditLogRecord, PartnerForecastException>> AddAuditLog(AuditLogRecord log, CancellationToken cancellationToken);
}
