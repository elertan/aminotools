using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoTools.Pages;
//using Newtonsoft.Json;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        public Page Page
        {
            get => _page;
            set
            {
                _page = value;
                OnPropertyChanged();
            }
        }

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
            IsBusy = true;

            var result = await App.Api.Global.S.Auth.Login(Username, Password);
            if (!result.DidSucceed())
            {
                IsBusy = false;
                await Page.DisplayAlert("Error", result.Info.Message, "Ok");
                return;
            }
            await SaveState();
            //await SettingsManager.SaveSetting(nameof(App) + nameof(App.Account),
            //    JsonConvert.SerializeObject(result.Data));

            //App.Account = result.Data;

            _alreadyLoggedInBefore = true;
            IsBusy = false;

            //await Page.Navigation.PushAsync(new HomePage(), true);
        }

        private async Task SaveState()
        {
            await SettingsManager.SaveSetting(nameof(LoginPageViewModel) + nameof(Username), Username);
            await SettingsManager.SaveSetting(nameof(LoginPageViewModel) + nameof(Password), Password);
        }

        private string _username;
        private string _password;
        private Command _loginCommand;
        private Page _page;

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
