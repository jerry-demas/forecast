namespace PartnerForecast.Website.Application.Features.Users.Models;

public record User(
    int EmployeeNumber,
    string EmployeeName,
    string Title,
    string EmailAddress,
    bool IsAdmin,
    bool IsInactive,
    string EmployeeDomain,
    bool IsEQRUser);

