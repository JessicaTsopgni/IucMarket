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
    public class ProductsViewModel : BaseViewModel
    {
        public ObservableCollection<ProductModel> Products { get; }
        public Command LoadProductsCommand { get; }
        public Command AddProductCommand { get; }
        public Command<ProductModel> ProductSelectedCommand { get; }
        public Command<ProductModel> StarTappedCommand { get; }

        private bool isFirstLoad;

        private LoginNamePageData loginNamePageData;
        public IProductDataStore ProductDataStore => DependencyService.Get<IProductDataStore>();
        public ISecureStorage SecureStorage => DependencyService.Get<ISecureStorage>();

        public ProductsViewModel()
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
            AddProductCommand = new Command(OnAddProduct);
            //Task.Run(async () => await ExecuteLoadProductsCommand());
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

        private async void OnAddProduct(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        private async void OnStarTapped(ProductModel product)
        {
            if (product == null)
                return;
            if (!SecureStorage.Exist(App.SessionKeyName))
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                selectedProduct = product;
                await Shell.Current.Navigation.PushPopupAsync(new RatePopup(SetStarsCount));
            }
        }

        async Task SetStarsCount(int numberOfStarSelected)
        {
            var owner = JsonConvert.DeserializeObject<UserModel>(CrossSecureStorage.Current.GetValue(App.SessionKeyName));

            await ProductDataStore.Rate(selectedProduct, owner, numberOfStarSelected);
        }

        async void OnProductSelected(ProductModel product)
        {
            try
            {

                if (product == null)
                    return;

                Application.Current.Properties[nameof(ProductsPageData)] = new ProductsPageData(loginNamePageData, product);

                // This will push the ProductDetailPage onto the navigation stack
                //await Shell.Current.GoToAsync($"/{nameof(ProductDetailPage)}");

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

    public class ProductsPageData
    {
        public LoginNamePageData LoginNamePageData { get; set; }
        public ProductModel Product { get; set; }

        public ProductsPageData()
        {

        }

        public ProductsPageData(LoginNamePageData loginNamePageData, ProductModel product)
        {
            LoginNamePageData = loginNamePageData;
            Product = product;
        }
    }
}