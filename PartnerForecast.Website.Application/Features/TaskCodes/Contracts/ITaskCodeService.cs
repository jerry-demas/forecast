using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.TaskCodes.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;
using PartnerForecast.Website.Application.Features.Users.Models;

namespace PartnerForecast.Website.Application.Features.TaskCodes.Contracts;

public interface ITaskCodeService
{
    Task<Either<IEnumerable<TaskCode>, PartnerForecastException>> GetTaskCodes(TaskCodeRequest request, CancellationToken cancellationToken);    
    Task<Either<TaskCode, PartnerForecastException>> GetTaskCodeByCode(string taskCode, CancellationToken cancellationToken);
    Task<Either<TaskCode, PartnerForecastException>> GetTaskCodeById(int id, CancellationToken cancellationToken);
    Task<Either<TaskCode, PartnerForecastException>> AddTaskCode(TaskCode taskCode, CancellationToken cancellationToken);
    Task<Either<TaskCode, PartnerForecastException>> UpdateTaskCode(TaskCode taskCode, CancellationToken cancellationToken);
    Task<Either<TaskCode, PartnerForecastException>> DeleteTaskCode(int Id, CancellationToken cancellationToken);

}