using Cbiz.SharedPackages;
using CBIZ.SharedPackages.Ldap;
using Microsoft.Extensions.Logging;
using PartnerForecast.Website.Application.Features.Users.Contracts;
using PartnerForecast.Website.Application.Features.Users.Models;
using PartnerForecast.Website.Application.Shared;

namespace PartnerForecast.Website.Application.Features.Users.Services;

public class PartnerForecastLdapService (
    LdapService ldapService,
    ILogger<PartnerForecastLdapService> logger
) : IPartnerForecastLdapService
{
    private readonly LdapService _ldapService = ldapService;
    private readonly ILogger<PartnerForecastLdapService> _logger = logger;
    
    
    
    public async Task<Either<List<LdapUser>, PartnerForecastException>> FindUsersByName(string name, CancellationToken cancellationToken)
    {
        try
        {
            
            if (string.IsNullOrEmpty(name)) return new PartnerForecastException("User name not provided.");

            var result = await SearchLdap($"*{name}*");
            
            var users = ExtractUsers(result, name);
            if(users.HasFailure)
                return new PartnerForecastException(users.Failure);

            return users;
        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex.Message);
        }
    }

    public async Task<Either<LdapUser, PartnerForecastException>> GetCurrentUserByIdentityAsync(string userIdentity, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Looking for user {UserIdentity}", userIdentity);

        try
        {
            userIdentity = RegexReplace.UserIdentityCleanupRegex().Replace(userIdentity ?? string.Empty, "");
            userIdentity = userIdentity.Replace("@AD.CBIZ.COM", "");
            var result = await SearchLdap(userIdentity);
            var users = ExtractUsers(result, userIdentity);
            if (users.HasFailure)
                return users.Failure;

            if (users.Value is null || users.Value.Count == 0)
            {
                return new PartnerForecastException("No users found");
            }
            return users.Value[0];


        }
        catch (Exception ex)
        {
            return new PartnerForecastException(ex, $"Error retrieving current user {userIdentity}");
        }

        
    }

     private async Task<LdapSearchResult> SearchLdap(string userName)
 {
     if (userName.Contains('*'))
         return await FindUsersByNameAsync(userName);

     return await GetUserAttributesAsync(userName);

 }

    private async Task<LdapSearchResult> GetUserAttributesAsync(string userName)
    {

        return await _ldapService.GetUserAttributesAsync(userName,
            FilterAttributes.Values.Email |
            FilterAttributes.Values.EmployeeId |
            FilterAttributes.Values.LegacyEmployeeId |
            FilterAttributes.Values.FullName |
            FilterAttributes.Values.Domain |
            FilterAttributes.Values.IsEnabled |
            FilterAttributes.Values.Title
            );

    }
    
    private async Task<LdapSearchResult> FindUsersByNameAsync(string userName)
    {

        return await _ldapService.FindUserByNameAsync(userName,
            FilterAttributes.Values.Email |
            FilterAttributes.Values.EmployeeId |
            FilterAttributes.Values.LegacyEmployeeId |
            FilterAttributes.Values.FullName |
            FilterAttributes.Values.Domain |
            FilterAttributes.Values.IsEnabled |
            FilterAttributes.Values.Title
            );

    }


    private Either<List<LdapUser>, PartnerForecastException> ExtractUsers(
        LdapSearchResult ldapResults,
        string searchName)
    {
        List<LdapUser> users = new List<LdapUser>();

        if (ldapResults.IsNotFound)
        {
            _logger.LogWarning("user {UserIdentity} not found", searchName);
            return new PartnerForecastException($"Unable to find user for {searchName}");
        }
        if (ldapResults.HasFailure)
        {
            _logger.LogWarning("Issue calling ldap: {ErrorMessage}", ldapResults.ErrorMessage);
            return new PartnerForecastException(ldapResults.ErrorMessage);
        }

        foreach (var entry in ldapResults.Entries)
        {
            users.Add(new LdapUser
            {
                EmailAddress = entry.Value(FilterAttributes.Values.Email),
                EmployeeId = entry.Value(FilterAttributes.Values.EmployeeId).IntFromString(),
                LMEmployeeId = entry.Value(FilterAttributes.Values.EmployeeId).IntFromString(),
                FullName = entry.Value(FilterAttributes.Values.FullName),
                Id = searchName.Contains('*') ? string.Empty : searchName,
                Domain = entry.Value(FilterAttributes.Values.Domain).Replace(".com", "").ToLower(),
                IsActive = bool.Parse(entry.Value(FilterAttributes.Values.IsEnabled)),
                Title = entry.Value(FilterAttributes.Values.Title)
            });
        }

        return users;
    }
    
}
