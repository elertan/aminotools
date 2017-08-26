using System;
using System.Threading.Tasks;
using AminoTools.Pages;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Auth
{
    public class LoginPageViewModel : BaseViewModel
    {
        private static bool _alreadyLoggedInBefore;

        public LoginPageViewModel()
        {
            LoginCommand = new Command(DoLogin);

            Username = SettingsManager.GetSettingWithFallback(nameof(LoginPageViewModel) + nameof(Username), "");
        }

        public override void Initialize()
        {
            if (!_alreadyLoggedInBefore)
            {
                var savedUsername = SettingsManager.GetSettingWithFallback(nameof(LoginPageViewModel) + nameof(Username), "");
                var savedPassword = SettingsManager.GetSettingWithFallback(nameof(LoginPageViewModel) + nameof(Password), "");
                if (!MayLogin
                    && !string.IsNullOrWhiteSpace(savedUsername)
                    && !string.IsNullOrWhiteSpace(savedPassword))
                {
                    Username = savedUsername;
                    Password = savedPassword;
                }
                if (MayLogin) DoLogin();
            }
        }

        private async void DoLogin()
        {
            var result = await DoAsBusyState(App.Api.Global.S.Auth.Login(Username, Password));
            if (!result.DidSucceed())
            {
                await Page.DisplayAlert("Error", result.Info.Message, "Ok");
                return;
            }
            await DoAsBusyState(SaveStateAsync());
            await DoAsBusyState(SettingsManager.SaveSettingAsync(nameof(App) + nameof(App.Account),
                JsonConvert.SerializeObject(result.Data)));

            App.Account = result.Data;

            _alreadyLoggedInBefore = true;

            App.MainPage = new MainPage();
        }

        private async Task SaveStateAsync()
        {
            await SettingsManager.SaveSettingAsync(nameof(LoginPageViewModel) + nameof(Username), Username);
            await SettingsManager.SaveSettingAsync(nameof(LoginPageViewModel) + nameof(Password), Password);
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
