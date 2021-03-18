using Acr.UserDialogs;
using IucMarket.Mobile.Common;
using IucMarket.Mobile.Models;
using IucMarket.Mobile.Services;
using IucMarket.Mobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IucMarket.Mobile.ViewModels
{
    public class LoginCodeViewModel : BaseViewModel
    {
        public int MAX_CODE_VALIDATION_LENGTH { get => 5; }
        public Command ValidateCodeCommand { get; }
        public Command ResendCodeCommand { get; }
        public Command ChangeNumberCommand { get; }

        private string codeValidation;
        public string CodeValidation
        {
            get { return codeValidation; }
            set 
            {
                SetProperty(ref codeValidation, value, onChanged: validate);
            }
        }

        private string formattedPhoneMumber;
        public string FormattedPhoneMumber
        {
            get { return formattedPhoneMumber; }
            set
            {
                SetProperty(ref formattedPhoneMumber, value);
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref isBusy, value, onChanged: ChangeCanExecute);
            }
        }

        private void ChangeCanExecute()
        {
            ValidateCodeCommand?.ChangeCanExecute();
            ResendCodeCommand?.ChangeCanExecute();
            ChangeNumberCommand?.ChangeCanExecute();
        }

       private LoginPageData loginPageData;
        IUserDataStore OwnerDataStore => DependencyService.Get<IUserDataStore>();
        ISecureStorage SecureStorage => DependencyService.Get<ISecureStorage>();

        private DateTime startDate;

        public LoginCodeViewModel()
        {
            loginPageData = Application.Current.Properties.FirstOrDefault(x => x.Key == nameof(LoginPageData)).Value as LoginPageData;
            if (loginPageData == null)
            {
                Task.Run(async () => await Shell.Current.GoToAsync($"//{nameof(LoginPage)}"));
                return;
            }
            
                
            CodeValidation = string.Empty;

            startDate = DateTime.UtcNow;
            //validate();
        }



        private void validate()
        {
            IsBusy = !long.TryParse(CodeValidation, out _) || CodeValidation?.Length < MAX_CODE_VALIDATION_LENGTH;
        }
    }

    public class LoginCodePageData
    {
        public LoginPageData LoginPageData { get; set; }
        public string CodeValidation { get; set; }
        public LoginCodePageData()
        {

        }

        public LoginCodePageData(LoginPageData loginPageData, string codeValidation)
        {
            LoginPageData = loginPageData;
            CodeValidation = codeValidation;
        }
    }
}
