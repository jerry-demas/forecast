namespace PartnerForecast.Website.Application.Features.AuditLogs.Models;

public record Change(
    string Field,
    string ValueFrom,
    string ValueTo
);