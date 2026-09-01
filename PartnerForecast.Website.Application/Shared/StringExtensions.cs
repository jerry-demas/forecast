namespace PartnerForecast.Website.Application.Shared;
internal static class StringExtensions
{
    public static string TimeStamp(this string value, TimeProvider timeProvider) => $"{value} - {timeProvider.GetUtcNow().ToString(format: "yyyy-MM-dd HH:mm:ss")}";
    public static string TimeStamp(this string value) => TimeStamp(value, TimeProvider.System);

    public static int IntFromString(this string value)
    {
        if (string.IsNullOrEmpty(value)) return 0;

        if (int.TryParse(value, out int parsedId)) return parsedId;
        else return 0;

    }

}

