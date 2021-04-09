using IucMarket.Mobile.Views;
using Xamarin.Forms;

namespace IucMarket.Mobile.ViewModels
{
    public class SignSwitchViewModel : BaseViewModel
    {
        public Command OnSignInCommand { get; }
        public Command OnSignUpCommand { get; }
        public SignSwitchViewModel()
        {
            OnSignInCommand = new Command(OnSignIn);
            OnSignUpCommand = new Command(OnSignUp);
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
