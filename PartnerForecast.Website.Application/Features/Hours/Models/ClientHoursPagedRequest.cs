using PartnerForecast.Website.Application.Shared.Modules;

namespace PartnerForecast.Website.Application.Features.Hours.Models;

public record ClientHoursPagedRequest(
        ClientHoursRequest hourRequest,
        int PageNumber = 1,
        int PageSize = 50,               
        string? SortField = null,
        bool SortDescending = false       
);
