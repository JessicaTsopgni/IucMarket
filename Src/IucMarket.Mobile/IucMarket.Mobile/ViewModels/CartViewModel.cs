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
using System.Collections.Generic;

namespace IucMarket.Mobile.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private OrderModel cart;
        public OrderModel Cart
        {
            get => cart;
            set
            {
                SetProperty(ref cart, value);
            }
        }

        private bool showTimesIcon;
        public bool ShowTimesIcon
        {
            get => showTimesIcon;
            set
            {
                SetProperty(ref showTimesIcon, value);
            }
        }

        private bool showOrderIcon;
        public bool ShowOrderIcon
        {
            get => showOrderIcon;
            set
            {
                SetProperty(ref showOrderIcon, value);
            }
        }

        public Command LoadProductsCommand { get; }
        public Command RemoveToCartCommand { get; }
        public Command ChangeQuantityToCartCommand { get; }
        public Command OrderCartCommand { get; }
        public Command GoBackCommand { get; }
        public Command SelectedAllToCartCommand { get; }
        public Command OnCartQuantityCommand { get; }
        private bool isFirstLoad;
        private CollectionView collectionView;

        //private LoginNamePageData loginNamePageData;
        public IProductDataStore ProductDataStore => DependencyService.Get<IProductDataStore>();
        public ISecureStorage SecureStorage => DependencyService.Get<ISecureStorage>();

       
        public CartViewModel()
        {     
            Title = "Cart";
            LoadProductsCommand = new Command(async () => await OnExecuteLoadProducts());
            RemoveToCartCommand = new Command(async () => await OnRemoveToCart());
            //ChangeQuantityToCartCommand = new Command<ProductModel>(OnChangeQuantityToCart);
            OrderCartCommand = new Command(OnOrderCart, () => ShowOrderIcon && !IsBusy);
            GoBackCommand = new Command(OnBackButtonPressed);
            SelectedAllToCartCommand = new Command(OnSelectedAllToCartCommand);
            OnCartQuantityCommand = new Command(OnCartQuantity);
        }

        private void OnCartQuantity(object obj)
        {
            Cart.RiseOnTotalPropertyChanged();
        }

        private void OnSelectedAllToCartCommand(object obj)
        {
            if (collectionView.SelectedItems.Count != Cart.Products.Count)
                collectionView.SelectedItems = new ObservableCollection<object>(Cart.Products);
            else
                collectionView.SelectedItems.Clear();
        }

        private void OnOrderCart()
        {
            throw new NotImplementedException();
        }
        public void OnSelectionChanged(CollectionView sender)
        {
            collectionView = sender;           
            ShowTimesIcon = collectionView?.SelectedItems.Count > 0;
        }
        async Task OnExecuteLoadProducts()
        {
            IsBusy = true;
            try
            {
                var json = CrossSecureStorage.Current.GetValue(App.SessionCartName);
                if (string.IsNullOrEmpty(json))
                    return;
                Cart = JsonConvert.DeserializeObject<OrderModel>(json);
                ShowOrderIcon = (Cart?.Products.Count ?? 0) > 0;
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
        }

        public void OnBackButtonPressed()
        {
            var json = JsonConvert.SerializeObject(Cart);
            CrossSecureStorage.Current.SetValue(App.SessionCartName, json);
            Shell.Current.GoToAsync("..");
        }


        private async Task OnRemoveToCart()
        {
            var productsToRemove = collectionView?.SelectedItems.Select(x => x as ProductModel).ToArray();
            if (productsToRemove == null)
                return;

            if (await UserDialogs.Instance.ConfirmAsync("Do you really want to delete selected product(s)?", "Confirm"))
            {
                foreach (var product in productsToRemove)
                    Cart.Remove(product);
            }

            collectionView?.SelectedItems.Clear();

            ShowOrderIcon = (Cart?.Products.Count ?? 0) > 0;
        }

    }
}