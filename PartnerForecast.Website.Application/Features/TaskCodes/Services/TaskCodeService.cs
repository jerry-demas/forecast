using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.TaskCodes.Contracts;
using PartnerForecast.Website.Application.Features.TaskCodes.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;
using System.Linq.Expressions;

namespace PartnerForecast.Website.Application.Features.TaskCodes.Services;

public class TaskCodeService(ITaskCodeRepository taskCodeRepository) : ITaskCodeService
{
    private readonly ITaskCodeRepository _taskCodeRepository = taskCodeRepository;
    
    public Task<Either<TaskCode, PartnerForecastException>> GetTaskCodeByCode(string taskCode, CancellationToken cancellationToken)
        => _taskCodeRepository.GetSingleAsync(
            t => t.Code == taskCode, 
            cancellationToken);

    public async Task<Either<TaskCode, PartnerForecastException>> GetTaskCodeById(int id, CancellationToken cancellationToken)
        => await _taskCodeRepository.GetSingleAsync(
            t => t.Id == id, 
            cancellationToken);
    
    public async Task<Either<IEnumerable<TaskCode>, PartnerForecastException>> GetTaskCodes(
        TaskCodeRequest request,
        CancellationToken cancellationToken)
    {

        Expression<Func<TaskCode, bool>> predicate = x =>
            (string.IsNullOrWhiteSpace(request.SearchText) ||
                x.Code.Contains(request.SearchText) ||
                x.CodeDescription.Contains(request.SearchText))
                &&
                (!request.ActiveOnly || x.IsActive);

        return await _taskCodeRepository.GetListAsync(
            predicate,
            cancellationToken);
    }
    
    public async Task<Either<TaskCode, PartnerForecastException>> AddTaskCode(TaskCode taskCode, CancellationToken cancellationToken)
    {

        var codeExists = await _taskCodeRepository.GetSingleAsync(
            x => x.Code == taskCode.Code, 
            cancellationToken);

        if (codeExists.IsSuccess)
        {
            return new PartnerForecastException($"Task code {taskCode.Code} already exists.");
        }

        return await _taskCodeRepository.AddAsync(
            taskCode, 
            cancellationToken);
    }

    public async Task<Either<TaskCode, PartnerForecastException>> UpdateTaskCode(
        TaskCode taskCode, 
        CancellationToken cancellationToken)
        => await _taskCodeRepository.UpdateAsync(
            x => x.Id == taskCode.Id, 
            x =>            
            {  
                x.Code = taskCode.Code;
                x.CodeDescription = taskCode.CodeDescription;
                x.IsActive = taskCode.IsActive;
                x.Sort = taskCode.Sort;                
            },
            cancellationToken);

    public async Task<Either<TaskCode, PartnerForecastException>> DeleteTaskCode(
        int Id, 
        CancellationToken cancellationToken)
        => await _taskCodeRepository.UpdateAsync(
            x => x.Id == Id, 
            x => x.IsActive = false,
            cancellationToken);
}
