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
using IucMarket.Common;
using System.Net.Http;

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
        public Command OnOrderQuantityCommand { get; }
        private bool isFirstLoad;
        private CollectionView collectionView;

        //private LoginNamePageData loginNamePageData;
        public IOrderDataStore OrderDataStore => DependencyService.Get<IOrderDataStore>();
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
            OnOrderQuantityCommand = new Command(OnOrderQuantity);
        }

        private void OnOrderQuantity(object obj)
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

        private async void OnOrderCart()
        {
            if (await UserDialogs.Instance.ConfirmAsync($"You order will be submit.\n{Cart.TotalWithCurrency}", "Confirm"))
            {
                if (App.IsAuthenticate)
                {
                    await Shell.Current.Navigation.PushPopupAsync(new DeliveryPlacePopup(SetDeliveryPlace));
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("You need to sign in", "Warning");
                    await Shell.Current.GoToAsync($"/{nameof(SignInPage)}", true);
                }
            }
        }

        public async Task<OrderModel> SubmitCart()
        {

            try
            {
                IsBusy = true;
                UserDialogs.Instance.ShowLoading();
                var cart = App.Get<OrderModel>();
                if (Cart.Products.Count == 0) return null;

                var customer = App.Get<UserModel>();
                cart.CustomerId = customer.Id;

                var item = await OrderDataStore.AddAsync(cart);
                return item;

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
                UserDialogs.Instance.HideLoading();
            }
            return null;
        }

        async Task SetDeliveryPlace(DeliveryPlaceModel model)
        {
            Cart.Comment = model.Comment;
            await Task.FromResult(Cart.DeliveryPlace = (DeliveryPlaceOptions)int.Parse(model.Id));
            App.Save(Cart);

            var order = await SubmitCart();
            if (order != null)
            {
                App.Clear<OrderModel>();
                Shell.Current.CurrentItem = Shell.Current.Items.FirstOrDefault(x => x.Route == nameof(OrderPage));
                //await Shell.Current.GoToAsync($"//{nameof(OrderPage)}", true);
            }

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
                Cart = App.Get<OrderModel>();
                ShowOrderIcon = (Cart?.Products.Count ?? 0) > 0;
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
            OrderCartCommand.ChangeCanExecute();
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
            App.Save(Cart);
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
            OrderCartCommand.ChangeCanExecute();
        }

    }
}