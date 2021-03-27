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
    public class SignUpViewModel : BaseViewModel
    {
        public Command SignUpCommand { get; }
        public Command SignInCommand { get; }

        private string registrationNumber;
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set
            {
                SetProperty(ref registrationNumber, value, onChanged: validate);
            }
        }
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

        private string phoneCountryCode;
        public string PhoneCountryCode
        {
            get { return phoneCountryCode; }
            set
            {
                SetProperty(ref phoneCountryCode, value, onChanged: validate);
            }
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

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value, onChanged: validate);
            }
        }
        public Command TogglePasswordCommand { get; }

        private bool hidePassword = true;
        public bool HidePassword
        {
            get => hidePassword;
            private set => SetProperty(ref hidePassword, value);
        }

        IUserDataStore UserDataStore => DependencyService.Get<IUserDataStore>();

        public SignUpViewModel()
        {
            PhoneCountryCode = "+237";
            SignUpCommand = new Command(OnSignUp, x => !IsBusy);
            SignInCommand = new Command(OnSignIn, x => !IsBusy);
            TogglePasswordCommand = new Command(OnTogglePassword);
        }

        private async void OnSignIn(object obj)
        {
            await Shell.Current.GoToAsync($"/{nameof(SignInPage)}", true);
        }
        private void OnTogglePassword()
        {
            HidePassword = !HidePassword;
        }

        public void OnAppearing()
        {
            
        }

        private async void OnSignUp(object obj)
        {
            try
            {
                IsBusy = true;
                var user = await UserDataStore.AddAsync
                (
                    new UserModel
                    (
                        Email,
                        RegistrationNumber,
                        PhoneCountryCode,
                        long.Parse(PhoneNumber),
                        name,
                        Password
                    )
                );
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
            }
        }

        private void validate()
        {
            try
            {

                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                IsBusy = string.IsNullOrEmpty(Email) &&
                (!string.IsNullOrEmpty(Email) && !regex.IsMatch(Email)) &&                
                string.IsNullOrEmpty(Name) &&
                string.IsNullOrEmpty(PhoneNumber) &&
                string.IsNullOrEmpty(RegistrationNumber) &&
                string.IsNullOrEmpty(Password);
            }
            catch(Exception ex)
            {
                IsBusy = false;
            }
        }
    }

}
