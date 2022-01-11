using System;
using Planetwatcher.Utilities;
using Xamarin.Forms;

namespace Planetwatcher
{
    public partial class MainPage : ContentPage
    {
        public string ConnectionText { get; set; }
        
        public MainPage()
        {
            InitializeComponent();
            ConnectionText = $"Connecting to: {Endpoints.API}";
            BindingContext = this;
        }

        private async void Login(object sender, EventArgs e)
        {
            var isRegistered = await BackendService.IsRegistered(App.NotificationToken);
            if (!isRegistered)
            {
                await BackendService.RegisterNotificationToken(App.NotificationToken);
            }

            await Navigation.PushAsync(new Homepage());
        }

        private void ConnectionSettings(object sender, EventArgs e)
        {
            
        }
    }
}