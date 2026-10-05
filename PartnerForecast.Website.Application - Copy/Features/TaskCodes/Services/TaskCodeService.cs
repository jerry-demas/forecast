using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Features.AuditLogs.Contracts;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Contracts;
using PartnerForecast.Website.Application.Features.TaskCodes.Models;
using PartnerForecast.Website.Application.Features.TaskCodes.Modules;
using PartnerForecast.Website.Application.Features.Users.Models;
using PartnerForecast.Website.Application.Shared;
using System.Linq.Expressions;


namespace PartnerForecast.Website.Application.Features.TaskCodes.Services;

public class TaskCodeService(
    ITaskCodeRepository taskCodeRepository,
    IAuditLogService auditLogService
    ) : ITaskCodeService
{
    private readonly ITaskCodeRepository _taskCodeRepository = taskCodeRepository;
    private readonly IAuditLogService _auditLogService = auditLogService;    
    
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
    
    public async Task<Either<TaskCode, PartnerForecastException>> AddTaskCode(TaskCode taskCode, User currentUser, CancellationToken cancellationToken)
    {

        var codeExists = await _taskCodeRepository.GetSingleAsync(
            x => x.Code == taskCode.Code, 
            cancellationToken);

        if (codeExists.IsSuccess)
        {
            return new PartnerForecastException($"Task code {taskCode.Code} already exists.");
        }


        var addedTaskCode = await _taskCodeRepository.AddAsync(
            taskCode, 
            cancellationToken);
            
        if(addedTaskCode.HasFailure )
        {
            return addedTaskCode.Failure;
        }

         await AddAuditLog(
           new AuditLogRecord(
                0,
                addedTaskCode.Value.Id,
                ForecastConstants.AuditLog.Category.TaskCodes.ToString(),
                currentUser.EmployeeNumber,
                ForecastConstants.AuditLog.Action.Added.ToString(),
                [],
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                currentUser.EmployeeName,
                currentUser.EmployeeDomain),
            currentUser, 
            cancellationToken);

        return addedTaskCode.Value;
    }

    public async Task<Either<TaskCode, PartnerForecastException>> UpdateTaskCode(
        TaskCode taskCode, 
        User currentUser,
        CancellationToken cancellationToken)
        {
            
        var updatedTaskCodeResult = await _taskCodeRepository.UpdateAsync(
            x => x.Id == taskCode.Id,
            x =>            
            {  
                x.Code = taskCode.Code;
                x.CodeDescription = taskCode.CodeDescription;
                x.IsActive = taskCode.IsActive;
                x.Sort = taskCode.Sort;                
            }, 
            cancellationToken);

        if (updatedTaskCodeResult.HasFailure)
        {
            return updatedTaskCodeResult.Failure;
        }
    
        await AddAuditLog(
           new AuditLogRecord(
                0,
                taskCode.Id,
                ForecastConstants.AuditLog.Category.TaskCodes.ToString(),
                currentUser.EmployeeNumber,
                ForecastConstants.AuditLog.Action.Updated.ToString(),                
                updatedTaskCodeResult.Value.Changes,                        
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                currentUser.EmployeeName,
                currentUser.EmployeeDomain),
            currentUser, 
            cancellationToken);

        return updatedTaskCodeResult.Value.UpdatedTaskCode;
    }


    public async Task<Either<TaskCode, PartnerForecastException>> DeleteTaskCode(
        int Id, 
        User currentUser,
        CancellationToken cancellationToken)
    {
        
        var updateResult = await _taskCodeRepository.UpdateAsync(
            x => x.Id == Id, 
            x => x.IsActive = false,
            cancellationToken);
        if(updateResult.HasFailure)
        {
            return updateResult.Failure;
        }
        
        await AddAuditLog(
            new AuditLogRecord(
                0,
                Id,
                ForecastConstants.AuditLog.Category.TaskCodes.ToString(),
                currentUser.EmployeeNumber,
                ForecastConstants.AuditLog.Action.Deleted.ToString(),
                [],
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                currentUser.EmployeeName,
                currentUser.EmployeeDomain),            
            currentUser,
            cancellationToken);

        return updateResult.Value.UpdatedTaskCode;

    }


     

    private async Task<Either<Possible, PartnerForecastException>> AddAuditLog(
        AuditLogRecord record, 
        User currentUser,
        CancellationToken cancellationToken)
       {
        
            var result = await _auditLogService.AddAuditLog(
                record,
                cancellationToken);

            if(result.HasFailure)
            {
                return result.Failure;
            }

            return Possible.Completed;

        }

}
