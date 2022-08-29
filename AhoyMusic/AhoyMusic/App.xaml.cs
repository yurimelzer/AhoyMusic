using AhoyMusic.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AhoyMusic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPageDetail());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
