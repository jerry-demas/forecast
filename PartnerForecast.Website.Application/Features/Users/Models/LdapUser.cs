namespace PartnerForecast.Website.Application.Features.Users.Models;

public record LdapUser
{
    public required string EmailAddress { get; init; }
    public required int EmployeeId { get; init; }
    public required int LMEmployeeId { get; init; }
    public required string FullName { get; init; }
    public required string Id { get; init; }
    public string Domain { get; init; } = string.Empty;
    public required bool IsActive { get; init; }
    public required string Title { get; init;  }

    public string EmployeeName
    {       
        get
        {
            string[] name = FullName.Split(' ');
            string returnValue = name.Length == 3 ?
                $"{name[1]} {name[2]} {name[0].Replace(",", "")}" :
                $"{name[1]} {name[0].Replace(",", "")}";
            
            return returnValue;
        }
    }
}
