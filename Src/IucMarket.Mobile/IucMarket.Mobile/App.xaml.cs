using IucMarket.Mobile.Models;
using IucMarket.Mobile.Services;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace IucMarket.Mobile
{
    public partial class App : Application
    {
        public static string ApiAddress;
        //public static readonly string SessionKeyName = "UserSession";
        //public static readonly string SessionCartName = "CartSession";
        public static readonly string Name = "IUC Market";
        //public static readonly bool NeedToSubmitCart = false;
        public App()
        {
            InitializeComponent();
            //if (Xamarin.Essentials.DeviceInfo.DeviceType == Xamarin.Essentials.DeviceType.Virtual)
            //    ApiAddress = "http://192.168.127.1:8096";
            //else
                ApiAddress = "https://iucmarket.azurewebsites.net";

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<ProductDataStore>();
            DependencyService.Register<UserDataStore>();
            DependencyService.Register<SecureStorage>();
            DependencyService.Register<OrderDataStore>();

           //ISecureStorage SecureStorage = DependencyService.Get<ISecureStorage>();

            //SecureStorage.Remove(SessionCartName);
            

            MainPage = new AppShell();
        }
        public static bool HasNetwork =>
            Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.None;

        public static void Save<T>(T item) where T : new()
        {
            if (item == null) return;
            ISecureStorage SecureStorage = DependencyService.Get<ISecureStorage>();
            SecureStorage.Set(typeof(T).Name, JsonConvert.SerializeObject(item));
        }

        public static T Get<T>() where T : new()
        {
            ISecureStorage SecureStorage = DependencyService.Get<ISecureStorage>();
            var json = SecureStorage.Get(typeof(T).Name);
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<T>(json)
                : new T();
        }

        public static void Clear<T>() where T : new()
        {
            ISecureStorage SecureStorage = DependencyService.Get<ISecureStorage>();
            SecureStorage.Set(typeof(T).Name, JsonConvert.SerializeObject(new T()));
        }

        public static bool IsAuthenticate => Get<UserModel>()?.IsAuthenticate ?? false;

        public static void SignOut()
        {
            var user = Get<UserModel>();
            user.Token = string.Empty;
            Save(user);
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
