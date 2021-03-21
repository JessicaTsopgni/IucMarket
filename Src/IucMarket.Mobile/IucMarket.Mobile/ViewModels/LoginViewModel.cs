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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IucMarket.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

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

        IUserDataStore OwnerDataStore => DependencyService.Get<IUserDataStore>();
        ISecureStorage SecureStorage => DependencyService.Get<ISecureStorage>();

        public LoginViewModel()
        {
           
            LoginCommand = new Command(OnLoginClicked, x => !IsBusy);

            //validate();
        }

        public void OnAppearing()
        {
            Email = SecureStorage.Get("Email");
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

        private void validate()
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            IsBusy = !regex.IsMatch(Email) && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password);
        }
    }

}
