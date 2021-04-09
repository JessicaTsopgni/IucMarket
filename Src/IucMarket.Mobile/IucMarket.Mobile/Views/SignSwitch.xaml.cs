using IucMarket.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IucMarket.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignSwitch : ContentView
    {
        SignSwitchViewModel _viewModel;
        public SignSwitch()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SignSwitchViewModel();
        }
    }
}