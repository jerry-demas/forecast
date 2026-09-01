using Microsoft.Extensions.Options;

namespace PartnerForecast.Website.Application.Features.Blurbs;

public record BlurbOptions
{
    public string BlurbPrefix { get; init; } = string.Empty;
}

/// <summary>
/// Validation logic for BlurbOptions.
/// </summary>
public class BlurbOptionsValidation : IValidateOptions<BlurbOptions>
{
    public ValidateOptionsResult Validate(string? name, BlurbOptions options)
    {
        if (options.BlurbPrefix.StartsWith("Jim"))
        {
            return ValidateOptionsResult.Fail($"{nameof(BlurbOptions)}: Leave Jim out of this.");
        }

        return ValidateOptionsResult.Success;
    }
}
