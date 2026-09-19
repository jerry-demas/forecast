using PartnerForecast.Website.Application.Features.AuditLogs.Models;


namespace PartnerForecast.Website.Application.Features.Hours.Models;

/*
public record ClientHours(
    int Id, 
    int EmployeeNumber, 
    string EmployeeNameAssigned, 
    string EmployeeDomainAssigned, 
    string CustomerName, 
    string CustomerNumber,
    int Month, 
    int Year, 
    int Hours, 
    bool IsDeleted, 
    DateTime CreatedDateTime,
    DateTime LastUpdatedDateTime, 
    string? TaskCode, 
    bool isEQR, 
    bool isNonBillable, 
    int EmployeeNumberAssigned)
{   
    public ICollection<AuditLogTable> AuditLogs { get; set; } = new List<AuditLogTable>();
}
*/
public class ClientHours
{
    public int Id { get; set; }

    public required int EmployeeNumber { get; set; }

    public required string EmployeeNameAssigned { get; set; }

    public required string EmployeeDomainAssigned { get; set; }

    public required string CustomerName { get; set; }

    public required string CustomerNumber { get; set; }

    public required int Month { get; set; }

    public required int Year { get; set; }

    public required int Hours { get; set; }

    public required bool IsDeleted { get; set; }

    public required DateTime CreatedDateTime { get; set; }

    public required DateTime LastUpdatedDateTime { get; set; }
    
    public required string TaskCode { get; set; }

    public required bool IsEQR { get; set; }

    public required bool IsNonBillable { get; set; }
    public required int EmployeeNumberAssigned { get; set; }
    //public ICollection<AuditLogTable> AuditLogs { get; set; } = new List<AuditLogTable>();
}