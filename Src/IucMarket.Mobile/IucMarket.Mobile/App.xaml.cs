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
        //public static readonly string SessionKeyName = "UserSession";
        //public static readonly string SessionCartName = "CartSession";
        public static readonly string Name = "IUC Market";

        public App()
        {
            InitializeComponent();


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
            SecureStorage.Set(nameof(T), JsonConvert.SerializeObject(item));
        }

        public static T Get<T>() where T : new()
        {
            ISecureStorage SecureStorage = DependencyService.Get<ISecureStorage>();
            var json = SecureStorage.Get(nameof(T));
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<T>(json)
                : new T();
        }

        public static bool IsAuthenticate => !string.IsNullOrEmpty(Get<UserModel>()?.Token);

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
