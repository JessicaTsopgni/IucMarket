using IucMarket.Mobile.ViewModels;
using Xamarin.Forms;

namespace IucMarket.Mobile.Views
{
    public partial class ProductDetailPage : ContentPage
    {
        ProductDetailViewModel _viewModel;
        public ProductDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ProductDetailViewModel();
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing();
            base.OnAppearing();
        }
    }
}