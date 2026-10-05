namespace PartnerForecast.Website.Application.Features.TaskCodes.Models;

public record TaskCodeRequest(
    string? SearchText = null,   
    bool ActiveOnly = false
);