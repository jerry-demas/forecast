using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using System.Linq.Expressions;

namespace PartnerForecast.Website.Application.Features.AuditLogs.Contracts;

public interface IAuditLogRepository
{    
    
    Task<Either<AuditLogRecord, PartnerForecastException>> AddAsync(
        AuditLogRecord log, 
        CancellationToken cancellationToken);

    Task<Either<IEnumerable<AuditLogRecord>, PartnerForecastException>> GetListAsync(
        Expression<Func<AuditLogTable, bool>> predicate,       
        CancellationToken cancellationToken);

}
