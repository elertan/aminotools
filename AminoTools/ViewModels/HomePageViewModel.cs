using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Pages;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        private Command _testButtonCommand;

        public HomePageViewModel()
        {
            Initialize += HomePageViewModel_Initialize;
        }

        public Command TestButtonCommand
        {
            get => _testButtonCommand;
            set
            {
                _testButtonCommand = value; 
                OnPropertyChanged();
            }
        }

        private void HomePageViewModel_Initialize(object sender, EventArgs e)
        {
            TestButtonCommand = new Command(DoNavigateToTestPage);
        }

        private async void DoNavigateToTestPage()
        {
            await App.MainNavigation.PushAsync(new TestPage(), true);
        }
    }
}
