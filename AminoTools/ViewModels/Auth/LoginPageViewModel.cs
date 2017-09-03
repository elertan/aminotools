using System;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Auth;
using AminoTools.Pages;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts.Auth;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Auth
{
    public class LoginPageViewModel : BaseViewModel, ILoginPageViewModel
    {
        private readonly IAuthorizationProvider _authorizationProvider;

        public LoginPageViewModel(IAuthorizationProvider authorizationProvider)
        {
            _authorizationProvider = authorizationProvider;
            LoginCommand = new Command(DoLogin);

            Username = SettingsManager.GetSettingWithFallback(SettingsManager.AvailableSettings.Username, "");
        }

        private async void DoLogin()
        {
            Account account = null;
            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Logging in";

                account = await _authorizationProvider.Login(Username, Password);
                if (account == null) return;

                IsBusyData.Description = "Saving settings";
                await DoAsBusyState(SaveStateAsync());
            });

            if (account == null)
            {
                await Page.DisplayAlert("Whoops", "Logging in failed", "Ok");
                return;
            }

            // Set authentication on api
            await App.Login(account);
        }

        private async Task SaveStateAsync()
        {
            await SettingsManager.SaveSettingAsync(SettingsManager.AvailableSettings.Username, Username);
            await SettingsManager.SaveSettingAsync(SettingsManager.AvailableSettings.Password, Password);
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
