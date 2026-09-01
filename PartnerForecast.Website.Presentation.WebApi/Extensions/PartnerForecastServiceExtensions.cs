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
            string? clientConnectionString)
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


        /*
        services.AddDbContext<ExtensionDbContext>(options =>
        options.UseSqlServer(appSettingsOptions?.BatchExtensionConnectionString));

        services.AddDbContext<InTappWorkspacesIntegrationDbContext>(options =>
            options.UseSqlServer(inTappWorkspacesIntegrationConnectionString));

        services.AddScoped<IExtensionRepository, BatchExtensionRepository>();

        services.AddScoped<IBulkExtensionsOneCBizRepository, OneCBizRepository>();

        services.AddScoped<IEngagementWorkspaceConfigRepository, EngagementWorkspaceConfigRepository>();

        services.AddScoped<IAdditiveRepository, AdditiveRepository>();

        var gfrEndPointsSection = configuration.GetRequiredSection("GfrEndPointOptions");
        services.Configure<GfrEndPointOptions>(gfrEndPointsSection);

        var gfrAccessInfo = configuration.GetRequiredSection("GfrApiOptions");
        services.Configure<GfrApiOptions>(gfrAccessInfo);

        var gfrProcessInfo = configuration.GetRequiredSection("GfrProcessOptions");
        services.Configure<GfrProcessOptions>(gfrProcessInfo);

        services.AddScoped<ApiHelper>();
        services.AddScoped<MailService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IGfrService, GfrService>();

        services.AddHttpClient(appSettingsOptions?.HttpClientName ?? string.Empty,
            client =>
            {
                client.BaseAddress = new Uri(appSettingsOptions?.BatchExtensionBaseUrl ?? string.Empty);
            })
            .AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                TimeSpan.FromSeconds(1), 5)));

        services.AddHttpClient(appSettingsOptions?.AdditiveHttpClientName ?? string.Empty,
            client =>
            {
                client.BaseAddress = new Uri(appSettingsOptions?.AdditiveBaseUrl ?? string.Empty);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseDefaultCredentials = true
            })
            .AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                TimeSpan.FromSeconds(1), 5)));
        */

        return services;

        }
}
