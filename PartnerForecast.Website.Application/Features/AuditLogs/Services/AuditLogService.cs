using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.AuditLogs.Contracts;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Contracts;
using PartnerForecast.Website.Application.Features.TaskCodes.Services;

namespace PartnerForecast.Website.Application.Features.AuditLogs.Services;

public class AuditLogService (IAuditLogRepository auditLogRepository) : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository = auditLogRepository;

    public Task<Either<AuditLogRecord, PartnerForecastException>> AddAuditLog(AuditLogRecord log, CancellationToken cancellationToken)
        => _auditLogRepository.AddAuditLog(log, cancellationToken);

    public Task<Either<IEnumerable<AuditLogRecord>, PartnerForecastException>> GetAuditLogsByHoursId(int hoursId, CancellationToken cancellationToken)
        => _auditLogRepository.GetAuditLogsByHoursId(hoursId, cancellationToken);

    public Task<Either<IEnumerable<AuditLogRecord>, PartnerForecastException>> GetAuditLogsByisEQRisNonbillable(GetLogsRequest request, CancellationToken cancellationToken)
        => _auditLogRepository.GetAuditLogsByisEQRisNonbillable(request, cancellationToken);





}
