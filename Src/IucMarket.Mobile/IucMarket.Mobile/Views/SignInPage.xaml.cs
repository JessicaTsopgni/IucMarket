﻿using IucMarket.Mobile.ViewModels;
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
    public partial class SignInPage : ContentPage
    {
        SignInViewModel _viewModel;
        public SignInPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new SignInViewModel();;
        }

        protected override void OnAppearing()
        {
             base.OnAppearing();
           _viewModel.OnAppearing();
        }
    }
}