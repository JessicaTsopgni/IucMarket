using IucMarket.Mobile.Services;
using IucMarket.Mobile.Views;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IucMarket.Mobile
{
    public partial class App : Application
    {
        public static readonly string ApiAddress = "http://localhost:44372";
        public static readonly string SessionKeyName = "UserSession";
        public static readonly string Name = "IUC Market";
        public App()
        {
            InitializeComponent();


            DependencyService.Register<MockDataStore>();

            DependencyService.Register<ProductDataStore>();
            DependencyService.Register<UserDataStore>();
            DependencyService.Register<SecureStorage>();

            MainPage = new AppShell();
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
