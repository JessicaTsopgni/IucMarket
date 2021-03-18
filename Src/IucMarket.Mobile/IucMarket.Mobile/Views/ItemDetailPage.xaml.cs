using IucMarket.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace IucMarket.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}