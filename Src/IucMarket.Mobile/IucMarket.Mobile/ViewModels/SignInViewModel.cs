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
    public class SignInViewModel : BaseViewModel
    {
        public Command SignInCommand { get; }
        public Command SignUpCommand { get; }

        private string email;
        public string Email
        {
            get { return email; }
            set 
            {
                SetProperty(ref email, value, onChanged: validate);
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set 
            {
                SetProperty(ref password, value, onChanged: validate);
            }
        }

        IUserDataStore UserDataStore => DependencyService.Get<IUserDataStore>();

        public SignInViewModel()
        {

            SignInCommand = new Command(OnSignIn, x => !IsBusy);
            SignUpCommand = new Command(OnSignUp, x => !IsBusy);

            //validate();
        }


        private async void OnSignUp(object obj)
        {
            await Shell.Current.GoToAsync($"/{nameof(SignUpPage)}", true);
        }
        public void OnAppearing()
        {
            Email = App.Get<UserModel>().Email;
        }

        private async void OnSignIn(object obj)
        {
            try
            {
                IsBusy = true;
                UserDialogs.Instance.ShowLoading();
                var user = await UserDataStore.LoginAsync(Email, Password);
                App.Save(user);
                await Shell.Current.GoToAsync("..");
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

        private void validate()
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            IsBusy = !regex.IsMatch(Email) && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password);
        }
    }

}
