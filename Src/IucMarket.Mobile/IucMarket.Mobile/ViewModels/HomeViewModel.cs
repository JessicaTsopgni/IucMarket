using Acr.UserDialogs;
using IucMarket.Mobile.Common;
using IucMarket.Mobile.Models;
using IucMarket.Mobile.Services;
using IucMarket.Mobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using Plugin.SecureStorage;

namespace IucMarket.Mobile.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<ProductModel> Products { get; }
        public Command LoadProductsCommand { get; }
        public Command AddToCartCommand { get; }
        public Command<ProductModel> ProductSelectedCommand { get; }
        public Command<ProductModel> StarTappedCommand { get; }

        private bool isFirstLoad;

        //private LoginNamePageData loginNamePageData;
        public IProductDataStore ProductDataStore => DependencyService.Get<IProductDataStore>();
        public ISecureStorage SecureStorage => DependencyService.Get<ISecureStorage>();

        private Thickness bellBadgeMargin;
        public Thickness BellBadgeMargin 
        { 
            get => bellBadgeMargin; 
            set
            {
                SetProperty(ref bellBadgeMargin, value);
            }
        }

        private string bellBadgeText;
        public string BellBadgeText
        {
            get => bellBadgeText;
            set
            {
                SetProperty(ref bellBadgeText, value);
            }
        }

        private Thickness cartBadgeMargin;
        public Thickness CartBadgeMargin
        {
            get => cartBadgeMargin;
            set
            {
                SetProperty(ref cartBadgeMargin, value);
            }
        }

        private string cartBadgeText;
        public string CartBadgeText
        {
            get => cartBadgeText;
            set
            {
                SetProperty(ref cartBadgeText, value);
            }
        }
        public HomeViewModel()
        {
            //loginNamePageData = Application.Current.Properties.FirstOrDefault(x => x.Key == nameof(LoginNamePageData)).Value as LoginNamePageData;
            //if (loginNamePageData == null)
            //{
            //    Task.Run(async () => await Shell.Current.GoToAsync($"//{nameof(LoginPage)}"));
            //}
            Title = App.Name;
            Products = new ObservableCollection<ProductModel>();
            LoadProductsCommand = new Command(async () => await ExecuteLoadProductsCommand());
            ProductSelectedCommand = new Command<ProductModel>(OnProductSelected);
            StarTappedCommand = new Command<ProductModel>(OnStarTapped);
            AddToCartCommand = new Command<ProductModel>(OnAddToCart);
            BellBadgeMargin = new Thickness(0, 18, 10, 0);
            CartBadgeMargin = new Thickness(0, 18, 0, 0);
            SetCartBadge();
        }

        private void SetCartBadge(OrderModel cart = null)
        {
            if (cart == null)
            {
                var json = CrossSecureStorage.Current.GetValue(App.SessionCartName);
                cart = JsonConvert.DeserializeObject<OrderModel>(json);
            }
            var count = cart?.Products.Count ?? 0;
            if (count > 0)
            {
                CartBadgeText = count > 9 ? "9+" : count.ToString();
                CartBadgeMargin = new Thickness(0, 5, 10, 0);
            }
        }

        async Task ExecuteLoadProductsCommand()
        {
            IsBusy = true;

            try
            {
                Products.Clear();
                var items = await ProductDataStore.GetAsync(true);
                foreach (var item in items)
                {
                    Products.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await UserDialogs.Instance.AlertAsync
                (
                    ex.Message,
                    "Error"
                );
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            if (!isFirstLoad)
            {
                IsBusy = true;
                isFirstLoad = true;
            }
            //SelectedProduct = null;
        }

        private ProductModel selectedProduct;
        public ProductModel SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                SetProperty(ref selectedProduct, value);
                //OnProductSelected(value);
            }
        }

        private void OnAddToCart(ProductModel product)
        {
            if (product == null)
                return;

            var json = CrossSecureStorage.Current.GetValue(App.SessionCartName);
            if (string.IsNullOrEmpty(json))
                return; // init cart to App class

            var cart = JsonConvert.DeserializeObject<OrderModel>(json);
            cart.Add(product);

            SetCartBadge(cart);

            json = JsonConvert.SerializeObject(cart);
            CrossSecureStorage.Current.SetValue(App.SessionCartName, json);

            //await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        private async void OnStarTapped(ProductModel product)
        {
            if (product == null)
                return;
            //if (!SecureStorage.Exist(App.SessionKeyName))
            //{
            //    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            //}
            //else
            //{
                selectedProduct = product;
                await Shell.Current.Navigation.PushPopupAsync(new RatePopup(SetStarsCount));
            //}
        }

        async Task SetStarsCount(int numberOfStarSelected)
        {
            var json = CrossSecureStorage.Current.GetValue(App.SessionKeyName);
            if (!string.IsNullOrEmpty(json))
            {
                var owner = JsonConvert.DeserializeObject<UserModel>(CrossSecureStorage.Current.GetValue(App.SessionKeyName));
                await ProductDataStore.Rate(selectedProduct, owner, numberOfStarSelected);
            }
        }

        async void OnProductSelected(ProductModel product)
        {
            try
            {

                if (product == null)
                    return;

                Application.Current.Properties[nameof(HomePageData)] = new HomePageData(product);

                // This will push the ProductDetailPage onto the navigation stack
                await Shell.Current.GoToAsync($"/{nameof(ProductDetailPage)}");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Shell.Current.DisplayAlert
                (
                    "Error",
                    "An error occured Please try again later",
                    "Cancel"
                );
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    public class HomePageData
    {
        public ProductModel Product { get; set; }

        public HomePageData()
        {

        }

        public HomePageData(ProductModel product)
        {
            Product = product;
        }
    }
}