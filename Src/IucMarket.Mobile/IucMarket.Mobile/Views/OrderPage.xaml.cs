using IucMarket.Mobile.ViewModels;
using Xamarin.Forms;

namespace IucMarket.Mobile.Views
{
    public partial class OrderPage : ContentPage
    {
        OrderViewModel _viewModel;
        public OrderPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new OrderViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}