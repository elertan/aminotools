using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.ViewModels.Contracts;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class TestPageViewModel : BaseViewModel, ITestPageViewModel
    {
        private Command _buttonCommand;

        public Command ButtonCommand
        {
            get => _buttonCommand;
            set
            {
                _buttonCommand = value; 
                OnPropertyChanged();
            }
        }

        public TestPageViewModel()
        {
            Initialize += TestPageViewModel_Initialize;
        }

        private void TestPageViewModel_Initialize(object sender, EventArgs e)
        {
            ButtonCommand = new Command(DoTest);  
        }

        private async void DoTest()
        {
            var result = await DoAsBusyState(App.Api.S.GetBlogsByUserIdAsync("x146561979", "9f8e3a79-03ca-4a25-bc95-3b257c765bad"));
            if (result.DidSucceed())
            {
                await Page.DisplayAlert("Succes!", "The test succeeded", "Yay!");
            }
            else
            {
                await Page.DisplayAlert("Whoops!", "Something seems to have gone wrong.", "RIP :(");
            }
        }
    }
}
