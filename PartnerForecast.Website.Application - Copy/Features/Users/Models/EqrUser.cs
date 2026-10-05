namespace PartnerForecast.Website.Application.Features.Users.Models;
/*
public record EqrUser(
    int Id,
    int EmployeeNumber ,
    string EmployeeName ,
    string Title ,
    string EmailAddress,
    bool IsAdmin ,
    bool IsInactive ,
    string EmployeeDomain 
);
*/
public class EqrUser
{
    public int Id { get; set; }
    public int EmployeeNumber { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsInactive { get; set; }
    public string EmployeeDomain { get; set; } = string.Empty;
}