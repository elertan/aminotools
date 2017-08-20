using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private static bool _alreadyLoggedInBefore;

        public LoginPageViewModel()
        {
            ReadyForInitialization += LoginPageViewModel_ReadyForInitialization;

            LoginCommand = new Command(DoLogin);

            Username = SettingsManager.GetSettingWithFallback(nameof(LoginPageViewModel) + nameof(Username), "");
        }

        private void LoginPageViewModel_ReadyForInitialization(object sender, EventArgs e)
        {
            if (!_alreadyLoggedInBefore)
            {
                if (MayLogin) DoLogin();
            }
        }

        private async void DoLogin()
        {
            IsBusy = true;

            var result = await App.Api.Global.S.Auth.Login(Username, Password);
            await SaveState();

            _alreadyLoggedInBefore = true;
            IsBusy = false;
        }

        private async Task SaveState()
        {
            await SettingsManager.SaveSetting(nameof(LoginPageViewModel) + nameof(Username), Username);
            await SettingsManager.SaveSetting(nameof(LoginPageViewModel) + nameof(Password), Password);
        }

        private string _username;
        private string _password;
        private Command _loginCommand;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MayLogin));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MayLogin));
            }
        }

        public bool MayLogin => Helpers.IsValidEmail(Username) && !String.IsNullOrWhiteSpace(Password);

        public Command LoginCommand
        {
            get => _loginCommand;
            set
            {
                _loginCommand = value;
                OnPropertyChanged();
            }
        }
    }
}
