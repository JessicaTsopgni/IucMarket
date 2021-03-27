using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using IucMarket.Mobile.Models;
using IucMarket.Mobile.Services;
using IucMarket.Mobile.Views;
using System.Net.Http;

namespace IucMarket.Mobile.ViewModels
{
    public class ProductDetailViewModel : BaseViewModel
    {

        public event EventHandler ScrolledToTop;
        public Command LoadProductsCommand { get; }
        public Command<ProductModel> StarTappedCommand { get; }
        public Command<ProductModel> AddToCartCommand { get; }
        public Command<ProductModel> ProductSelectedCommand { get; }

        private HomePageData homePageData;
        public HomePageData HomePageData
        {
            get => homePageData;
            set => SetProperty(ref homePageData, value);
        }


        public ObservableCollection<ProductModel> SameProducts { get; set; }
        private bool showSimilarProducts;
        public bool ShowSimilarProducts
        {
            get => showSimilarProducts;
            set => SetProperty(ref showSimilarProducts, value);
        }
        public IDataStore<ProductModel> ProductDataStore => DependencyService.Get<IDataStore<ProductModel>>();

        public ProductDetailViewModel()
        {
            HomePageData = Application.Current.Properties.FirstOrDefault(x => x.Key == nameof(HomePageData)).Value as HomePageData;
            ProductSelectedCommand = new Command<ProductModel>(OnProductSelected);
            LoadProductsCommand = new Command(async () => await ExecuteLoadProductsCommand());
            StarTappedCommand = new Command<ProductModel>(OnStarTapped);
            AddToCartCommand = new Command<ProductModel>(OnAddToCart);
            SameProducts = new ObservableCollection<ProductModel>();
        }

        private bool isFirstLoad = false;
        public void OnAppearing()
        {
            if (HomePageData == null)
            {
                Task.Run(async () => await Shell.Current.GoToAsync($"//{nameof(HomePage)}"));
                return;
            }
            if (!isFirstLoad)
            {
                IsBusy = true;
                isFirstLoad = true;
            }
            //SelectedProduct = null;
        }
        async Task ExecuteLoadProductsCommand()
        {
            IsBusy = true;

            try
            {
                SameProducts.Clear();
                var items = await ProductDataStore.GetAsync(HomePageData.Product);
                foreach (var item in items)
                {
                    SameProducts.Add(item);
                }
                ShowSimilarProducts = SameProducts.Count > 0;
            }
            catch (HttpRequestException ex)
            {
                await UserDialogs.Instance.AlertAsync
                (
                   ex.Message,
                   "Error"
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await UserDialogs.Instance.AlertAsync
                (
                   "An error occured.\nPlease try again later.",
                   "Error"
                );
            }
            finally
            {
                IsBusy = false;
            }
        }

        async void OnProductSelected(ProductModel product)
        {
            HomePageData = new HomePageData(product);
            await ExecuteLoadProductsCommand();
            ScrolledToTop?.Invoke(this, new EventArgs());
        }

        void OnStarTapped(ProductModel product)
        {
            new HomeViewModel().StarTappedCommand.Execute(product);
        }


        void OnAddToCart(ProductModel product)
        {
            new HomeViewModel().AddToCartCommand.Execute(product);
        }
    }
}
