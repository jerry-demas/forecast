namespace PartnerForecast.Website.Application.Features.Blurbs;
public class BlurbNotFoundException : Exception
{
    public BlurbNotFoundException(Guid blurbId) : base($"Cannot find Blurb {blurbId}")
    { }
}
