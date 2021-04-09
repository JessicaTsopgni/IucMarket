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
using System.Net.Http;

namespace IucMarket.Mobile.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        public ObservableCollection<OrderModel> Orders { get; }
        public Command LoadOrdersCommand { get; }
        public Command DeleteCommand { get; }
        private bool isFirstLoad;

        //private LoginNamePageData loginNamePageData;
        public IOrderDataStore OrderDataStore => DependencyService.Get<IOrderDataStore>();
        public ISecureStorage SecureStorage => DependencyService.Get<ISecureStorage>();


        public OrderViewModel()
        {           
            Title = App.Name;
            Orders = new ObservableCollection<OrderModel>();
            LoadOrdersCommand = new Command(async () => await ExecuteLoadOrdersCommand());
            DeleteCommand = new Command<OrderModel>(OnDelete);
        }


        async Task ExecuteLoadOrdersCommand()
        {
            IsBusy = true;

            try
            {
                Orders.Clear();
                var items = await OrderDataStore.GetAsync(true);
                foreach (var item in items)
                {
                    Orders.Add(item);
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

        public void  OnAppearing()
        {
            base.OnApparing();//for update auth
            if (IsAuthenticate)
            {
                if (!isFirstLoad)
                {
                    IsBusy = true;
                    isFirstLoad = true;
                }
            }
        }

        private void OnDelete(OrderModel product)
        {
            if (product == null)
                return;
        }

    }

}