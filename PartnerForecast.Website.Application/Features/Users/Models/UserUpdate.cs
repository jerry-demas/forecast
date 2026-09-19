using PartnerForecast.Website.Application.Features.AuditLogs.Models;

namespace PartnerForecast.Website.Application.Features.Users.Models;

public record UserUpdate(
    EqrUser UpdatedUser,
    Change[] Changes
);