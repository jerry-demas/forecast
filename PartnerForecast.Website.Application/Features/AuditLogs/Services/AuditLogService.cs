using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.AuditLogs.Contracts;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Contracts;
using PartnerForecast.Website.Application.Features.TaskCodes.Services;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace PartnerForecast.Website.Application.Features.AuditLogs.Services;

public class AuditLogService (IAuditLogRepository auditLogRepository) : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository = auditLogRepository;
    
    public Task<Either<AuditLogRecord, PartnerForecastException>> AddAuditLog(AuditLogRecord log, CancellationToken cancellationToken)
        => _auditLogRepository.AddAsync(log, cancellationToken);

    public Task<Either<IEnumerable<AuditLogRecord>, PartnerForecastException>> GetAuditLogs(
        GetLogsRequest request, 
        CancellationToken cancellationToken)   
        => _auditLogRepository.GetListAsync(
            x => x.ClientHoursId == request.Id &&
            x.LogCategory == request.Category.ToString(), 
            cancellationToken);      
}
