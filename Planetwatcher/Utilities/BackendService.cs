using System;
using System.Threading.Tasks;
using Planetwatcher.Model;
using Xamarin.Forms;

namespace Planetwatcher.Utilities
{
    public static class BackendService
    {
        private class Noop {}

        public static async Task<bool> IsRegistered(string token)
        {
            try
            {
                var response = await Connector.Post(Endpoints.CheckRegistration(), new TokenRegistration
                {
                    Token = token,
                    PlatformDetails = new PlatformDetails
                    {
                        IsPhone = Device.Idiom == TargetIdiom.Phone,
                        IsTablet = Device.Idiom != TargetIdiom.Phone,
                        IsAndroid = Device.RuntimePlatform == Device.Android,
                        IsIos = Device.RuntimePlatform == Device.iOS
                    }
                });
                return response is { IsSuccessStatusCode: true };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception during token registration check: " + ex);
                return false;
            }
        }
        
        public static async Task RegisterNotificationToken(string token)
        {
            try
            {
                var response = await Connector.Post(Endpoints.RegisterNotificationToken(), new TokenRegistration
                {
                    Token = token,
                    PlatformDetails = new PlatformDetails
                    {
                        IsPhone = Device.Idiom == TargetIdiom.Phone,
                        IsTablet = Device.Idiom != TargetIdiom.Phone,
                        IsAndroid = Device.RuntimePlatform == Device.Android,
                        IsIos = Device.RuntimePlatform == Device.iOS
                    }
                });
                var parsedResponse = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("Response from registration: " + parsedResponse);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception during token registration: " + ex);
            }
        }
    }
}