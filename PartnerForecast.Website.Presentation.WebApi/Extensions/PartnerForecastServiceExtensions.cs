using CBIZ.SharedPackages.Ldap;
using Microsoft.EntityFrameworkCore;
using PartnerForecast.Website.Application.Features.AuditLogs.Services;
using PartnerForecast.Website.Application.Features.AuditLogs.Contracts;
using PartnerForecast.Website.Application.Features.TaskCodes.Contracts;
using PartnerForecast.Website.Application.Features.TaskCodes.Services;
using PartnerForecast.Website.Application.Features.Users.Contracts;
using PartnerForecast.Website.Application.Features.Users.Services;
using PartnerForecast.Website.Application.Features.Hours.Contracts;
using PartnerForecast.Website.Application.Features.Hours.Services;
using PartnerForecast.Website.Application.Shared.Data;

using PartnerForecast.Website.Application.Features.Hours.Models;
using PartnerForecast.Website.Application.Features.Clients.Contracts;
using PartnerForecast.Website.Application.Features.Clients.Services;

namespace PartnerForecast.Website.Presentation.WebApi.Extensions;

public static class PartnerForecastServiceExtensions
{
    public static IServiceCollection AddPartnerForecastService(
            this IServiceCollection services, 
            IConfiguration configuration, 
            string? partnerExtensionConnectionString,
            string? clientConnectionString,
            string? loggingConnectionString)
        {
            
            
            // Fail fast: ensure a valid connection string is provided
            if (string.IsNullOrWhiteSpace(partnerExtensionConnectionString))
            {
                throw new ArgumentException("Connection string for PartnerForecastDataContext must not be null or empty.", nameof(partnerExtensionConnectionString));
            }
            if (string.IsNullOrWhiteSpace(clientConnectionString))
            {
                throw new ArgumentException("Connection string for ClientDataContext must not be null or empty.", nameof(clientConnectionString));
            }
            if (string.IsNullOrWhiteSpace(loggingConnectionString))
            {
                throw new ArgumentException("Connection string for LoggingDataContext must not be null or empty.", nameof(loggingConnectionString));
            }
            services.AddLdapService();
            services.AddScoped<IPartnerForecastLdapService, PartnerForecastLdapService>();
            services.AddScoped<IUserService, UserService>();            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaskCodeRepository, TaskCodeRepository>();
            services.AddScoped<ITaskCodeService, TaskCodeService>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IClientHoursRepository, ClientHoursRepository>();
            services.AddScoped<IClientHoursService, ClientHoursService>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientService, ClientService>();
            
            services.AddDbContext<PartnerForecastDataContext>(options =>
                   options.UseSqlServer(partnerExtensionConnectionString));

            services.AddDbContext<ClientDataContext>(options =>
                   options.UseSqlServer(clientConnectionString));
                

            return services;

        }
}
