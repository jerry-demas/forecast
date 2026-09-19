namespace PartnerForecast.Website.Application.Shared;

public static class ForecastConstants
{

    public static class UserDomains
    {
        public const string USER_DOMAIN_MARCUM = "mkllp";
        public const string USER_DOMAIN_CBIZ = "cbiz";
    }

    public static class AuditLog
    {
        public enum Category
        {
            ClientHours,
            TaskCodes,
            EqrUsers
        }
         public enum Action
        {
            Added,
            Updated,
            Deleted
        }
       
    }

    public const string CLIENT_STATUS_ACTIVE = "ACTIVE";


}
