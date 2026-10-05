
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;

namespace PartnerForecast.Website.Application.Features.Clients.Models;
public record ClientHoursUpdate(
    ClientHours UpdatedClientHour,
    Change[] Changes
);