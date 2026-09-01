namespace PartnerForecast.Website.Application.Features.Hours.Models;

public record ClientHoursPagedResponse(
    int pageNumber,
    int pageSize,
    int totalRecords,
    IEnumerable<ClientHours> records
);
