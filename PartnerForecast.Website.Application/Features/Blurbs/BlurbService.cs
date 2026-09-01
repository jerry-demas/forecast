using Cbiz.SharedPackages;
using PartnerForecast.Website.Application.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PartnerForecast.Website.Application.Features.Blurbs;

public static class BlurbServiceExtensions
{
    public static IServiceCollection AddBlurbService(this IServiceCollection services)
    {
        services.AddScoped<BlurbService>();
        services.AddSingleton<IValidateOptions<BlurbOptions>, BlurbOptionsValidation>();
        services.AddOptions<BlurbOptions>().BindConfiguration(nameof(BlurbOptions)).ValidateOnStart();

        services.AddScoped<BlurbRepository>();
        services.AddDbContext<BlurbDbContext>((options) =>
        {
            options.UseSqlite($"Filename=:memory:");
        }, ServiceLifetime.Singleton); // NOTE: This would normally be Scoped, this is only Singleton due to "in memory"

        return services;
    }
}

public class BlurbService
{
    private readonly BlurbOptions _config;
    private readonly ILogger<BlurbService> _logger;
    private readonly BlurbRepository _blurbRepository;

    public BlurbService(ILogger<BlurbService> logger, IOptions<BlurbOptions> config, BlurbRepository blurbRepository)
    {
        _logger = logger;
        _config = config.Value ?? new BlurbOptions();
        _blurbRepository = blurbRepository;
    }

    public Either<Guid, Exception> CreateBlurb(Blurb blurb)
    {
        _logger.LogInformation("Creating Blurb for user {UserId}", blurb.CreatedBy);

        if (!string.IsNullOrWhiteSpace(_config.BlurbPrefix))
        {
            blurb.Text = $"{_config.BlurbPrefix} : {blurb.Text}";
        }

        return _blurbRepository.CreateBlurb(blurb);
    }

    public Possible<Exception> UpdateBlurb(Blurb blurb)
    {
        _logger.LogInformation("Updating Blurb {BlurbId}", blurb.BlurbId);

        if (!string.IsNullOrWhiteSpace(_config.BlurbPrefix))
        {
            blurb.Text = $"{_config.BlurbPrefix} : {blurb.Text}";
        }

        return _blurbRepository.UpdateBlurb(blurb);
    }

    public Possible<Exception> DeleteBlurb(Guid blurbId)
    {
        _logger.LogInformation("Deleteing Blurb {BlurbId}".TimeStamp(), blurbId);

        return _blurbRepository.DeleteBlurb(blurbId);
    }

    public Either<Blurb, Exception> GetBlurb(Guid blurbId)
    {
        _logger.LogInformation("Get Blurb {BlurbId}", blurbId);
        return _blurbRepository.GetBlurb(blurbId);
    }

    public Either<List<Blurb>, Exception>  GetPublicBlurbs()
    {
        _logger.LogInformation("Get public Blurbs");
        return _blurbRepository.GetBlurbs(x => !x.IsPrivate);
    }

    public Either<List<Blurb>, Exception> GetUserBlurbs(string userId)
    {
        _logger.LogInformation("Get all Blurbs for user {UserId}", userId);
        return _blurbRepository.GetBlurbs(x => x.CreatedBy == userId);
    }
}

