using IucMarket.Mobile.Models;
using IucMarket.Mobile.Services;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace IucMarket.Mobile
{
    public partial class App : Application
    {
        public static readonly string ApiAddress = "http://192.168.127.1:8096";
        public static readonly string SessionKeyName = "UserSession";
        public static readonly string SessionCartName = "CartSession";
        public static readonly string Name = "IUC Market";

        public App()
        {
            InitializeComponent();


            DependencyService.Register<MockDataStore>();

            DependencyService.Register<ProductDataStore>();
            DependencyService.Register<UserDataStore>();
            DependencyService.Register<SecureStorage>();

            CrossSecureStorage.Current.DeleteKey(SessionCartName);
            var json = CrossSecureStorage.Current.GetValue(SessionCartName);
            if (string.IsNullOrEmpty(json))
            {
                var cart = new OrderModel();
                CrossSecureStorage.Current.SetValue(SessionCartName, JsonConvert.SerializeObject(cart));
            }

            MainPage = new AppShell();
        }
        public static bool HasNetwork =>
            Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.None;
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
