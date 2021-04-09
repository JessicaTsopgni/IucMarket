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
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IucMarket.Mobile.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public Command SendVerificationEmailCommand { get; }
        public Command OnSignOutCommand { get; }

        private UserModel user;
        public UserModel User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }
        private string emailVerified;
        public string EmailVerified
        {
            get { return emailVerified; }
            set { SetProperty(ref emailVerified, value); }
        }

        bool isNotEmailVerified;
        public bool IsNotEmailVerified
        {
            get { return isNotEmailVerified; }
            set { SetProperty(ref isNotEmailVerified, value); }
        }
        IUserDataStore UserDataStore => DependencyService.Get<IUserDataStore>();

        public ProfileViewModel()
        {
            SendVerificationEmailCommand = new Command(SendVerificationEmail);
            OnSignOutCommand = new Command(OnSignOut);
        }

        public void OnAppearing()
        {
            base.OnApparing();
            if (IsAuthenticate)
            {
                User = App.Get<UserModel>();
                IsNotEmailVerified = !User.IsEmailVerified;
                if (IsNotEmailVerified)
                    EmailVerified = "Email not verified.";
                else
                    EmailVerified = "Email verified !";
            }
            
        }

        private async void SendVerificationEmail()
        {
            try
            {
                IsBusy = true;
                UserDialogs.Instance.ShowLoading();
                await UserDataStore.SendEmailAsync
                (
                    User.Token
                );
                await UserDialogs.Instance.AlertAsync
                (
                   "Email sent !",
                   "Confirmation"
                );
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
                UserDialogs.Instance.HideLoading();

            }

        }

        private void OnSignOut(object obj)
        {
            App.SignOut();
            OnAppearing();
        }

        private async void OnSignIn()
        {
            await Shell.Current.GoToAsync($"/{nameof(SignInPage)}", true);
        }


        private async void OnSignUp()
        {
            await Shell.Current.GoToAsync($"/{nameof(SignUpPage)}", true);
        }

      
    }

}
