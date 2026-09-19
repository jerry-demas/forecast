using Cbiz.SharedPackages;
using Microsoft.Extensions.Logging;
using PartnerForecast.Website.Application.Features.AuditLogs.Contracts;
using PartnerForecast.Website.Application.Features.AuditLogs.Models;
using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.Users.Contracts;
using PartnerForecast.Website.Application.Features.Users.Models;
using PartnerForecast.Website.Application.Shared;
using System.Linq.Expressions;


namespace PartnerForecast.Website.Application.Features.Users.Services;

public class UserService(
    IPartnerForecastLdapService partnerForecastLdapService,
    IUserRepository _userRepository,
    IAuditLogService auditLogService,
    ILogger<UserService> _logger) : IUserService
{   
    private readonly IPartnerForecastLdapService _partnerForecastLdapService = partnerForecastLdapService;
    private readonly IUserRepository _userRepository = _userRepository;
    private readonly IAuditLogService _auditLogService = auditLogService;
    private readonly ILogger<UserService> _logger = _logger;

    public async Task<Either<List<User>, PartnerForecastException>> FindUsersByName(string name, CancellationToken cancellationToken)
    {
        if (name is null) return new PartnerForecastException($"Name is null");
        var ldapServiceResult = await _partnerForecastLdapService.FindUsersByName(name, cancellationToken);
        if(ldapServiceResult.HasFailure)
        {
            return ldapServiceResult.Failure;
        }
        var users = ldapServiceResult.Value.ToUsers(ForecastConstants.UserDomains.USER_DOMAIN_CBIZ);
        return users.ToList();
    }

    public async Task<Either<User, PartnerForecastException>> GetCurrentUserByIdentityAsync(string? userIdentity, CancellationToken cancellationToken)
    {

        if (userIdentity is null) return new PartnerForecastException($"User identity is null");

        try{

            var ldapServiceResult = await _partnerForecastLdapService.GetCurrentUserByIdentityAsync(userIdentity, cancellationToken);
            if(ldapServiceResult.HasFailure)
            {
                return ldapServiceResult.Failure;
            }

            bool isCbizUser = ldapServiceResult.Value.EmployeeId > 0;

            int employeeNumber = isCbizUser
                    ? ldapServiceResult.Value.EmployeeId
                    : ldapServiceResult.Value.LMEmployeeId;
            
            var userDomain = isCbizUser
                ? ForecastConstants.UserDomains.USER_DOMAIN_CBIZ
                : ForecastConstants.UserDomains.USER_DOMAIN_CBIZ;
            
            var eqrUser = await _userRepository.GetSingleAsync(
                x => x.EmployeeNumber == employeeNumber &&
                     x.EmployeeDomain == userDomain &&
                     !x.IsInactive,
                cancellationToken);

            if(eqrUser.HasFailure)
            {
                return eqrUser.Failure;
            }
            
            return new User(
                    EmployeeNumber: employeeNumber,
                    EmployeeName: eqrUser.Value.EmployeeName,
                    Title: eqrUser.Value.Title,
                    EmailAddress: eqrUser.Value.EmailAddress,
                    IsAdmin: eqrUser.Value.IsAdmin,
                    IsInactive: !ldapServiceResult.Value.IsActive,
                    EmployeeDomain: userDomain,
                    IsEQRUser: eqrUser is not null
                );                     
              
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving user from LDAP service.");
            return new PartnerForecastException($"An error occurred while retrieving user information.");
        }
        
    }
    
    public async Task<Either<List<EqrUser>, PartnerForecastException>> GetNaoUsers(NaoUsersRequest request, CancellationToken cancellationToken)
    {
        
        Expression<Func<EqrUser, bool>> predicate = x =>
            (string.IsNullOrWhiteSpace(request.SearchText) ||
                x.EmployeeName.Contains(request.SearchText) ||
                x.EmployeeNumber.ToString().Contains(request.SearchText))
            &&
            (!request.AdminOnly || x.IsAdmin)
            &&
            (!request.ActiveOnly || !x.IsInactive);

                return await _userRepository.GetListAsync(
                    predicate, 
                    cancellationToken);

    }


    public async Task<Either<EqrUser, PartnerForecastException>> DeleteNaoUser(
        int userId, 
        User currentUser,
        CancellationToken cancellationToken)
    {
        var updateResult = await _userRepository.UpdateAsync(
            x => x.Id == userId,
            x => x.IsInactive = true,
            cancellationToken);

        if(updateResult.HasFailure)
        {
            return updateResult.Failure;
        }
        
        await AddAuditLog(
            new AuditLogRecord(
                0,
                userId,
                ForecastConstants.AuditLog.Category.EqrUsers.ToString(),
                currentUser.EmployeeNumber,
                ForecastConstants.AuditLog.Action.Deleted.ToString(),
                [],
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                currentUser.EmployeeName,
                currentUser.EmployeeDomain),            
            currentUser,
            cancellationToken);

        return updateResult.Value.UpdatedUser;

    }


    public async Task<Either<EqrUser, PartnerForecastException>> UpdateNaoUser(
        EqrUser user, 
        User currentUser,
        CancellationToken cancellationToken)

    {
        
        var updateResult = await _userRepository.UpdateAsync(
            x => x.Id == user.Id,
            x =>            
            {   x.EmployeeNumber = user.EmployeeNumber;
                x.EmployeeName = user.EmployeeName;                
                x.Title = user.Title;
                x.EmailAddress = user.EmailAddress;
                x.IsAdmin = user.IsAdmin;
                x.IsInactive = user.IsInactive;
                x.EmployeeDomain = user.EmployeeDomain;
            },
            cancellationToken);

        if(updateResult.HasFailure)
        {
            return updateResult.Failure;
        }

        await AddAuditLog(
            new AuditLogRecord(
                0,
                user.Id,
                ForecastConstants.AuditLog.Category.EqrUsers.ToString(),
                currentUser.EmployeeNumber,
                ForecastConstants.AuditLog.Action.Updated.ToString(),
                updateResult.Value.Changes,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                currentUser.EmployeeName,
                currentUser.EmployeeDomain),            
            currentUser,
            cancellationToken);

        return updateResult.Value.UpdatedUser;

    }

    public async Task<Either<EqrUser, PartnerForecastException>> AddNaoUser(
        EqrUser user, 
        User currentUser,
        CancellationToken cancellationToken)
    {

        var userExists = await _userRepository.GetSingleAsync(
            x => x.EmployeeNumber == user.EmployeeNumber &&
                 x.EmployeeDomain == user.EmployeeDomain, 
            cancellationToken);

        if (userExists.IsSuccess)
        {
            return new PartnerForecastException($"User {user.EmployeeNumber} already exists.");
        }
        
        var addedUserresult =  await _userRepository.AddAsync(
            user, 
            cancellationToken);


        if(addedUserresult.HasFailure)
        {
            return addedUserresult.Failure;
        }

         await AddAuditLog(
           new AuditLogRecord(
                0,
                addedUserresult.Value.Id,
                ForecastConstants.AuditLog.Category.EqrUsers.ToString(),
                currentUser.EmployeeNumber,
                ForecastConstants.AuditLog.Action.Added.ToString(),
                [],
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                currentUser.EmployeeName,
                currentUser.EmployeeDomain),
            currentUser, 
            cancellationToken);


            return addedUserresult.Value;

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