using System.Text.RegularExpressions;

namespace PartnerForecast.Website.Application;

internal static partial class RegexReplace
{
    [GeneratedRegex(@"^.*\\")]
    public static partial Regex UserIdentityCleanupRegex();
       
}

