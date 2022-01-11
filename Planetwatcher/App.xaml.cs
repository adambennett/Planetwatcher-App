using System;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Planetwatcher
{
    public partial class App : Application
    {
        public static string NotificationToken;
        
        public App()
        {
            InitializeComponent();
            CrossFirebasePushNotification.Current.OnTokenRefresh += CurrentOnOnTokenRefresh;
            NotificationToken = CrossFirebasePushNotification.Current.Token;
            CrossFirebasePushNotification.Current.OnNotificationReceived += CurrentOnNotificationReceived;
            CrossFirebasePushNotification.Current.OnNotificationDeleted += CurrentOnNotificationDeleted;
            CrossFirebasePushNotification.Current.OnNotificationOpened += CurrentOnNotificationOpened;
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private static void CurrentOnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs e)
        {
            
        }

        private static void CurrentOnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            
        }

        private static void CurrentOnNotificationDeleted(object source, FirebasePushNotificationDataEventArgs e)
        {
            
        }

        private static void CurrentOnOnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            NotificationToken = e.Token;
        }
    }
}