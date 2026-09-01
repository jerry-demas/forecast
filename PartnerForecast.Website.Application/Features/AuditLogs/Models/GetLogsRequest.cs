using static PartnerForecast.Website.Application.Shared.ForecastConstants;

namespace PartnerForecast.Website.Application.Features.AuditLogs.Models;

public record GetLogsRequest(
    bool IsEqr,
    bool IsNonBillable,
    AuditLogCategories.Category Category);

