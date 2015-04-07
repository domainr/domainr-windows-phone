
namespace Domainr7.Model
{
    public static class Constants
    {
        public static string QueryUrl = "https://api.domainr.com/v1/api/json/search?q={0}";
        public static string InfoUrl = "https://api.domainr.com/v1/api/json/info?q={0}";
        public static string DomainrAppId = "windowsphone";

        // Messaging
        public static string DoSearchCommand = "DoTheSearchNowBruv";
        public static string SendDomainrDetailsCommand = "SendItOverMan";
        public static string ShareActionCommand = "ShareThatInfoOn";

        // "enums"
        public const string AvailabilityTaken = "taken";
        public const string AvailabilityMaybe = "maybe";
        public const string AvailabilityTLD = "tld";
        public const string AvailabilityAvailable = "available";
        public const string AvailabilityUnavailable = "unavailable";        
    }
}
