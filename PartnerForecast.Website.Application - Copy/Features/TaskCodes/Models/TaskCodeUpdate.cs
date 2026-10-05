
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;

namespace PartnerForecast.Website.Application.Features.TaskCodes.Models;
public record TaskCodeUpdate(
    TaskCode UpdatedTaskCode,
    Change[] Changes
);