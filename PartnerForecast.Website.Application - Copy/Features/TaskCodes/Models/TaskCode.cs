
namespace PartnerForecast.Website.Application.Features.TaskCodes.Modules;

public class TaskCode
{
    public int Id { get; set; }                    
    public required string Code { get; set; } 
    public required string CodeDescription { get; set; }
    public required bool IsActive { get; set; }
    public required int Sort { get; set; }
}