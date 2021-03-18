using IucMarket.Mobile.ViewModels;
using IucMarket.Mobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace IucMarket.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
