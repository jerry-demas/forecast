using Microsoft.AspNetCore.Authentication.Negotiate;
using NLog.Extensions.Logging;
using System.Text.Json.Serialization;


namespace PartnerForecast.Website.Presentation.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddHttpContextAccessor();

        //Register Partner Forecast Service
        services.AddPartnerForecastService(
            builder.Configuration,
            builder.Configuration.GetConnectionString("PartnerForecastConnection"),
            builder.Configuration.GetConnectionString("ClientConnection"),
            builder.Configuration.GetConnectionString("LoggingConnection"));

        // Alter NLOG section in "appsettings.config" file in this project to change logging behaviors
        // See https://nlog-project.org/ for more information
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddNLog(new NLogLoggingConfiguration(builder.Configuration.GetSection("NLog")));
        });

        // Add services to the container.
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddAuthorization();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactFrontend", policy =>
                {
                    policy.SetIsOriginAllowed(origin =>
                    {
                        // Allow any origin in development except for authenticated localhost
                        // If the origin is localhost, only allow if the request is authenticated
                        if (origin.StartsWith("http://localhost") || origin.StartsWith("https://localhost"))
                        {
                            // For localhost, require credentials (authentication)
                            return true;
                        }
                        // Allow all other origins (for development)
                        return true;
                    }).AllowCredentials()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();
        
        services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();
               
        return services;
    }
}
