namespace Planetwatcher.Utilities
{
    public static class Endpoints
    {
        public const string API = "http://192.168.1.31:80";
        public static string UserAPI = "";

        public static string RegisterNotificationToken()
        {
            return Api("/register-device");
        }

        public static string CheckRegistration()
        {
            return Api("/check-registration");
        }

        private static string Api(string endpoint)
        {
            return string.IsNullOrEmpty(UserAPI) ? $"{API}{endpoint}" : UserAPI;
        }
    }
}