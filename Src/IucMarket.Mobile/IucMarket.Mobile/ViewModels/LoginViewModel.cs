using Acr.UserDialogs;
using IucMarket.Mobile.Common;
using IucMarket.Mobile.Models;
using IucMarket.Mobile.Services;
using IucMarket.Mobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IucMarket.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command CountrySelectedCommand { get; }
        public Command WhatsAppLabelCommand { get; }
        public Command ChangeLanguageCommand { get; }

        private Color enButtonColor;
        public Color EnButtonColor
        {
            get { return enButtonColor; }
            set { SetProperty(ref enButtonColor, value); }
        }

        private Color frButtonColor;
        public Color FrButtonColor
        {
            get { return frButtonColor; }
            set { SetProperty(ref frButtonColor, value); }
        }

 

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set 
            {
                SetProperty(ref phoneNumber, value, onChanged: validate);
            }
        }

        private bool isWhatsApp;
        public bool IsWhatsApp
        {
            get { return isWhatsApp; }
            set 
            {
                SetProperty(ref isWhatsApp, value, onChanged: validate);
            }
        }

        private void ChangeCanExecute()
        {
            LoginCommand?.ChangeCanExecute();
            WhatsAppLabelCommand?.ChangeCanExecute();
            ChangeLanguageCommand?.ChangeCanExecute();
        }


        private LoginPageData loginPageData;

        IUserDataStore OwnerDataStore => DependencyService.Get<IUserDataStore>();
        ISecureStorage SecureStorage => DependencyService.Get<ISecureStorage>();

        public LoginViewModel()
        {
           
            LoginCommand = new Command(OnLoginClicked, x => !IsBusy);
            WhatsAppLabelCommand = new Command(OnWhatsAppLabelClicked);

            loginPageData = Application.Current.Properties.FirstOrDefault(x => x.Key == nameof(LoginPageData)).Value as LoginPageData;
            IsWhatsApp = loginPageData?.IsWhatsApp ?? false;
            PhoneNumber = loginPageData?.PhoneNumber;

            //validate();
        }

        public void OnAppearing()
        {
            //if (App.IsAuthenticate)
            //    await Shell.Current.GoToAsync($"//{nameof(TrucksPage)}");
        }


        private async void OnLoginClicked(object obj)
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await UserDialogs.Instance.AlertAsync
                (
                   "An_error_occured_Please_try_again_later",
                   "Error"
                );
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnWhatsAppLabelClicked(object obj)
        {
            IsWhatsApp = !IsWhatsApp;
        }


        private void validate()
        {
            IsBusy = false;
        }
    }

    public class LoginPageData
    {
        public string PhoneNumber { get; set; }
        public bool IsWhatsApp { get; set; }
        public LanguageOptions LanguageOptions { get; set; }
        public UserModel Owner { get; set; }
        public LoginPageData()
        {

        }

        public LoginPageData(string phoneNumber, bool isWhatsApp, UserModel owner)
        {
            PhoneNumber = phoneNumber;
            IsWhatsApp = isWhatsApp;
            Owner = owner;
        }
    }
}
