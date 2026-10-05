namespace PartnerForecast.Website.Application.Features.Hours.Models;

public record NaoUsersRequest(
    string? SearchText = null,
    bool AdminOnly = false,
    bool ActiveOnly = false
);


