

using PartnerForecast.Website.Application.Shared;

namespace PartnerForecast.Website.Application.Features.AuditLogs.Models;

public record GetLogsRequest(
    int Id,
    ForecastConstants.AuditLog.Category Category);
