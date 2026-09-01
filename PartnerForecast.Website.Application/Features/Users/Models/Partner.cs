namespace PartnerForecast.Website.Application.Features.Users.Models;

public record Partner(
    int EmployeeNumber, 
    string EmployeeName, 
    bool IsInactive, 
    string EmployeeDomain, 
    string Email, 
    string Title
);