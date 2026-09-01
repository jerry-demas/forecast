namespace PartnerForecast.Website.Application;

public class PartnerForecastException : Exception
{
    public PartnerForecastException() { }
    public PartnerForecastException(Exception ex) { }
    public PartnerForecastException(string message) : base(message) { }
    public PartnerForecastException(Exception ex, string message) { }
}
