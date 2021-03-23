using IucMarket.Mobile.Models;
using IucMarket.Mobile.ViewModels;
using IucMarket.Mobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IucMarket.Mobile.Views
{
    public partial class CartPage : ContentPage
    {
        CartViewModel _viewModel;

        public CartPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new CartViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}