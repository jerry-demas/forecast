namespace PartnerForecast.Website.Application.Shared;

public static class ForecastConstants
{

    public static class UserDomains
    {
        public const string USER_DOMAIN_MARCUM = "mkllp";
        public const string USER_DOMAIN_CBIZ = "cbiz";
    }

    public static class AuditLogCategories
    {
        public enum Category
        {
            ClientHours,
            TaskCode,
            EqrUsers
        }
    }

    public const string CLIENT_STATUS_ACTIVE = "ACTIVE";


}
