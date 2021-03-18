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
    public class LoginNameViewModel : BaseViewModel
    {
        public Command DoneCommand { get; }

        private string profileName;
        public string ProfileName
        {
            get { return profileName; }
            set 
            {
                SetProperty(ref profileName, value, onChanged: validate); 
            }
        }



        private void ChangeCanExecute()
        {
            DoneCommand?.ChangeCanExecute();
        }

        ISecureStorage SecureStorage => DependencyService.Get<ISecureStorage>();
        IUserDataStore OwnerDataStore => DependencyService.Get<IUserDataStore>();

        private UserModel owner;
        public LoginNameViewModel()
        {   
           ProfileName = string.Empty;
            //validate();
        }

        public async void OnAppearing()
        {
            if (!SecureStorage.Exist(App.SessionKeyName))
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }
            this.owner = JsonConvert.DeserializeObject<UserModel>(SecureStorage.Get(App.SessionKeyName));
            ProfileName = this.owner.Name;
        }



        private void validate()
        {
            IsBusy = string.IsNullOrWhiteSpace(ProfileName);
        }
    }

    public class LoginNamePageData
    {
        public LoginCodePageData LoginCodePageData { get; set; }
        public string ProfileName { get; set; }
        public LoginNamePageData()
        {

        }

        public LoginNamePageData(LoginCodePageData loginCodePageData, string profileName)
        {
            LoginCodePageData = loginCodePageData;
            ProfileName = profileName;
        }
    }
}
