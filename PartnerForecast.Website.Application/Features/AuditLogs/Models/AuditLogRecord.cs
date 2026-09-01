namespace PartnerForecast.Website.Application.Features.AuditLogs.Models;

public record AuditLogRecord(
    int Id,
    int ClientHoursId,
    string LogCategory,
    int ChangedByEmployeeNumber,
    string ChangeDescription,
    IEnumerable<Change>? Changes,
    DateTime CreatedDateTime,
    string ChangedByEmployeeName,
    string ChangedByEmployeeDomain
);
