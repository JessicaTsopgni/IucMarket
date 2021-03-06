﻿using Acr.UserDialogs;
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
using System.Net.Http;

namespace IucMarket.Mobile.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<ProductModel> Products { get; }
        public Command LoadProductsCommand { get; }
        public Command AddToCartCommand { get; }
        public Command<ProductModel> ProductSelectedCommand { get; }
        public Command<ProductModel> StarTappedCommand { get; }
        public Command CartTappedCommand { get; }
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
            CartTappedCommand = new Command(OnCartTapped);
            AddToCartCommand = new Command<ProductModel>(OnAddToCart);
            BellBadgeMargin = new Thickness(0, 18, 10, 0);
            CartBadgeMargin = new Thickness(0, 18, 0, 0);
        }

        private void SetCartBadge(OrderModel cart = null)
        {
            if (cart == null)
                 cart = App.Get<OrderModel>();

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
                SetCartBadge();
                var items = await ProductDataStore.GetAsync(true);
                foreach (var item in items)
                {
                    Products.Add(item);
                    SetProductCartQuantity(item);
                }
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

        public void OnAppearing()
        {
            if (!isFirstLoad)
            {
                IsBusy = true;
                isFirstLoad = true;
            }
            else
            {
                SetCartBadge();
                foreach(var item in Products)
                {
                    SetProductCartQuantity(item);
                }
            }
        }

        private static void SetProductCartQuantity(ProductModel item)
        {
            var cart = App.Get<OrderModel>();
            item.OrderQuantity = cart.Products?.FirstOrDefault(x => x.Id == item.Id)?.OrderQuantity ?? 0;
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

            var cart = App.Get<OrderModel>();
            cart.Add(product);

            SetCartBadge(cart);

            App.Save(cart);
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
            var owner = App.Get<UserModel>();
            await ProductDataStore.Rate(selectedProduct, owner, numberOfStarSelected);
        }

        async void OnProductSelected(ProductModel product)
        {
            try
            {

                if (product == null)
                    return;

                Application.Current.Properties[nameof(HomePageData)] = new HomePageData(product);

                // This will push the ProductDetailPage onto the navigation stack
                await Shell.Current.GoToAsync($"/{nameof(ProductDetailPage)}", true);

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

        async void OnCartTapped()
        {
            try
            {
                // This will push the ProductDetailPage onto the navigation stack
                await Shell.Current.GoToAsync($"/{nameof(CartPage)}", true);

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
    }

    public class HomePageData
    {
        public ProductModel Product { get; set; }
        public ObservableCollection<ProductModel> Products { get; set; }

        public HomePageData()
        {

        }
        public HomePageData(ObservableCollection<ProductModel> products)
        {
            Products = products;
        }

        public HomePageData(ProductModel product)
        {
            Product = product;
        }
    }
}