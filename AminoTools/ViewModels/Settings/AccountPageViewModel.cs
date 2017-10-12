using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.ViewModels.Contracts.Settings;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Settings
{
    public class AccountPageViewModel : BaseViewModel, IAccountPageViewModel
    {
        private Command _logOutCommand;

        public AccountPageViewModel()
        {
            Initialize += AccountPageViewModel_Initialize;

            LogOutCommand = new Command(DoLogOut);
        }

        public Command LogOutCommand
        {
            get => _logOutCommand;
            set
            {
                _logOutCommand = value; 
                OnPropertyChanged();
            }
        }

        private void AccountPageViewModel_Initialize(object sender, EventArgs e)
        {
            
        }

        private async void DoLogOut()
        {
            await App.Logout();
        }
    }
}
