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
        public Command LoadProductsCommand { get; }
        public Command RemoveToCartCommand { get; }
        public Command ChangeQuantityToCartCommand { get; }
        public Command OrderCartCommand { get; }

        private bool isFirstLoad;

        //private LoginNamePageData loginNamePageData;
        public IProductDataStore ProductDataStore => DependencyService.Get<IProductDataStore>();
        public ISecureStorage SecureStorage => DependencyService.Get<ISecureStorage>();

       
        public CartViewModel()
        {     
            Title = "Cart";
            LoadProductsCommand = new Command(async () => await ExecuteLoadProductsCommand());
            RemoveToCartCommand = new Command<ProductModel>(OnRemoveToCart);
            //ChangeQuantityToCartCommand = new Command<ProductModel>(OnChangeQuantityToCart);
            OrderCartCommand = new Command(OnOrderCart);
        }

        private void OnOrderCart()
        {
            throw new NotImplementedException();
        }

        async Task ExecuteLoadProductsCommand()
        {
            IsBusy = true;
            try
            {
                var json = CrossSecureStorage.Current.GetValue(App.SessionCartName);
                if (string.IsNullOrEmpty(json))
                    return;
                Cart = JsonConvert.DeserializeObject<OrderModel>(json); 
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

        private void OnRemoveToCart(ProductModel product)
        {
            if (product == null)
                return;
            Cart.Remove(product);
        }

    }
}