using IucMarket.Mobile.Services;
using IucMarket.Mobile.Views;
using System;
using System.Linq;
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

        private void ChangeCanExecute()
        {
            ValidateCodeCommand?.ChangeCanExecute();
            ResendCodeCommand?.ChangeCanExecute();
            ChangeNumberCommand?.ChangeCanExecute();
        }

        IUserDataStore OwnerDataStore => DependencyService.Get<IUserDataStore>();
        ISecureStorage SecureStorage => DependencyService.Get<ISecureStorage>();

        private DateTime startDate;

        public LoginCodeViewModel()
        {
           
                
            CodeValidation = string.Empty;

            startDate = DateTime.UtcNow;
            //validate();
        }



        private void validate()
        {
            IsBusy = !long.TryParse(CodeValidation, out _) || CodeValidation?.Length < MAX_CODE_VALIDATION_LENGTH;
        }
    }

}
