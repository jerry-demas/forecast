namespace PartnerForecast.Website.Application.Features.Blurbs;

public class BlurbSqlException : Exception
{
    public BlurbSqlException(Exception exception): base ("An error occurred processing the SQL command. Please see inner excpetion for details", exception)
    {
    }

    public BlurbSqlException(string message) : base($"An error occurred processing the SQL command. {message}")
    {
    }
}
