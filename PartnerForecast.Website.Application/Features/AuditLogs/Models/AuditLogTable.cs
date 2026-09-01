using PartnerForecast.Website.Application.Features.Hours.Models;

namespace PartnerForecast.Website.Application.Features.AuditLogs.Models;

public record AuditLogTable(
    int Id,
    int ClientHoursId,
    string LogCategory,
    int ChangedByEmployeeNumber,
    string ChangeDescription,
    string? Changes, //JSON Data
    DateTime CreatedDateTime,
    string ChangedByEmployeeName,
    string ChangedByEmployeeDomain
)
{
    public ClientHours ClientHours { get; init; } = default!;
};


