namespace WebApiGateway.Versions;

public class ApiInfo
{
    private static class Constants
    {
        public const string V1 = "v1";
        public const string CatchAll = "{**catch-all}";
    }

    public static class EventApi
    {
        public const string EventClusterId = "event";
        public const string EventV1PathTransform = $"/api/{Constants.V1}/match/{Constants.CatchAll}";
    }

    public static class IdentityApi
    {
        public const string IdentityClusterId = "identity";
        public const string IdentityV1PathTransform = $"/api/{Constants.V1}/identity/{Constants.CatchAll}";
    }

    public static class SlipApi
    {
        public const string SlipClusterId = "slip";
        public const string IdentityV1PathTransform = $"/api/{Constants.V1}/slip/{Constants.CatchAll}";
    }
}