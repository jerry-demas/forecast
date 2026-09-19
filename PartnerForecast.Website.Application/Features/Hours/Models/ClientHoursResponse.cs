public class ClientHoursResponse
{
    public int Id { get; set; }
    public int EmployeeNumber { get; set; }
    public string EmployeeNameAssigned { get; set; } = string.Empty;
    public string EmployeeDomainAssigned { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerNumber { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public int Hours { get; set; }
    public string? TaskCode { get; set; }
    public string? TaskCodeDescription { get; set; }
    public bool HasAuditLog { get; set; }
    public bool IsEQR { get; set; }
    public bool IsNonBillable { get; set; }
    public int EmployeeNumberAssigned { get; set; }
}