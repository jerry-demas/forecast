using Cbiz.SharedPackages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PartnerForecast.Website.Application.Features.AuditLogs.Contracts;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.Hours.Contracts;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.Users.Models;
using PartnerForecast.Website.Application.Shared;

namespace PartnerForecast.Website.Application.Features.Hours.Services;

public class ClientHoursService(
    IClientHoursRepository clientHoursRepository,
    IAuditLogService auditLogService) : IClientHoursService
{
    private readonly IClientHoursRepository _clientHoursRepository = clientHoursRepository;
    private readonly IAuditLogService _auditLogService = auditLogService;

    public Task<Either<ClientHoursPagedResponse, PartnerForecastException>> GetClientHours(
        ClientHoursPagedRequest request, 
        CancellationToken cancellationToken)                      
        => _clientHoursRepository.GetListPagedAsync(
            request, 
            cancellationToken);
    
    public Task<Either<ClientHours, PartnerForecastException>> GetClientHoursById(
        int id, 
        CancellationToken cancellationToken)
        => _clientHoursRepository.GetSingleAsync(
            ch => ch.Id == id, 
            cancellationToken);

    public async Task<Either<ClientHours, PartnerForecastException>> AddHour(
        ClientHours hours,
        User currentUser, 
        CancellationToken cancellationToken)


    {
        var addedHourResponse = await _clientHoursRepository.AddAsync(
            hours, 
            cancellationToken);

        if(addedHourResponse.HasFailure )
        {
            return addedHourResponse.Failure;
        }

        await AddAuditLog(
           new AuditLogRecord(
                0,
                addedHourResponse.Value.Id,
                ForecastConstants.AuditLog.Category.ClientHours.ToString(),
                currentUser.EmployeeNumber,
                ForecastConstants.AuditLog.Action.Added.ToString(),
                [],
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                currentUser.EmployeeName,
                currentUser.EmployeeDomain),
            currentUser, 
            cancellationToken);

        return addedHourResponse.Value;
    
    }
       

    public async Task<Either<ClientHours, PartnerForecastException>> DeleteHours(
        int hoursId, 
        User currentUser,
        CancellationToken cancellationToken)
    {
        var updateResult = await _clientHoursRepository.UpdateAsync(
            ch => ch.Id == hoursId,
            ch => ch.IsDeleted = true, 
            cancellationToken);

        if(updateResult.HasFailure)
        {
            return updateResult.Failure;
        }

        await AddAuditLog(
            new AuditLogRecord(
                0,
                hoursId,
                ForecastConstants.AuditLog.Category.ClientHours.ToString(),
                currentUser.EmployeeNumber,
                ForecastConstants.AuditLog.Action.Deleted.ToString(),
                [],
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                currentUser.EmployeeName,
                currentUser.EmployeeDomain),            
            currentUser,
            cancellationToken);
            
        return updateResult.Value.UpdatedClientHour;

    }


    public async Task<Either<ClientHours, PartnerForecastException>> UpdateHours(
        ClientHours hours, 
        User currentUser,
        CancellationToken cancellationToken)
    {
        
        var updateResult = await _clientHoursRepository.UpdateAsync(
            ch => ch.Id == hours.Id,
            ch => 
            {
                ch.EmployeeNumber = hours.EmployeeNumber;
                ch.CustomerName = hours.CustomerName;
                ch.CustomerNumber = hours.CustomerNumber;
                ch.Month = hours.Month;
                ch.Year = hours.Year;
                ch.Hours = hours.Hours;                
                ch.IsDeleted = hours.IsDeleted;
                ch.LastUpdatedDateTime = DateTime.Now;
                ch.TaskCode = hours.TaskCode;
                ch.IsEQR = hours.IsEQR;
                ch.IsDeleted = hours.IsDeleted;
                ch.IsNonBillable = hours.IsNonBillable;
                ch.EmployeeNameAssigned = hours.EmployeeNameAssigned;
                ch.EmployeeDomainAssigned = hours.EmployeeDomainAssigned;
                ch.EmployeeNumberAssigned = hours.EmployeeNumberAssigned;                                
            },
            cancellationToken);

        if(updateResult.HasFailure)
        {
            return updateResult.Failure;
        }


         await AddAuditLog(
           new AuditLogRecord(
                0,
                hours.Id,
                ForecastConstants.AuditLog.Category.ClientHours.ToString(),
                currentUser.EmployeeNumber,
                ForecastConstants.AuditLog.Action.Updated.ToString(),                
                updateResult.Value.Changes,                        
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                currentUser.EmployeeName,
                currentUser.EmployeeDomain),
            currentUser, 
            cancellationToken);


        return updateResult.Value.UpdatedClientHour;    
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
