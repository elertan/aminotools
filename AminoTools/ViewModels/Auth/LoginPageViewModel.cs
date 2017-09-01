using System;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Auth;
using AminoTools.Pages;
using AminoTools.ViewModels.Contracts.Auth;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Auth
{
    public class LoginPageViewModel : BaseViewModel, ILoginPageViewModel
    {
        public LoginPageViewModel()
        {
            LoginCommand = new Command(DoLogin);

            Username = SettingsManager.GetSettingWithFallback(SettingsManager.AvailableSettings.Username, "");
        }

        private async void DoLogin()
        {
            ApiResult<Account> result = null;
            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Logging in";
                var r = await App.Api.Login(Username, Password);
                result = r;
                if (!r.DidSucceed()) return;

                IsBusyData.Description = "Saving settings";
                await DoAsBusyState(SaveStateAsync());
            });

            if (!result.DidSucceed())
            {
                await Page.DisplayAlert("Whoops", result.Info.Message, "Ok");
                return;
            }

            // Set authentication on api
            await App.Login(result.Data);
        }

        private async Task SaveStateAsync()
        {
            await SettingsManager.SaveSettingAsync(SettingsManager.AvailableSettings.Username, Username);
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
