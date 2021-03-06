﻿using IucMarket.Mobile.Models;
using IucMarket.Mobile.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IucMarket.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeliveryPlacePopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private Func<DeliveryPlaceModel, Task> setCallback;
        DeliveryPlaceViewModel _viewModel;
        public DeliveryPlacePopup(Func<DeliveryPlaceModel, Task> setCallback)
        {
            InitializeComponent();
            BindingContext = _viewModel = new DeliveryPlaceViewModel();
            this.setCallback = setCallback;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }

        private async void Done_Clicked(object sender, EventArgs e)
        {
            DeliveryPlaceModel model = picker.SelectedItem as DeliveryPlaceModel;
            model.Comment = comment.Text;
            await this.setCallback(model);
            await Shell.Current.Navigation.PopPopupAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopPopupAsync();
        }
    }
}