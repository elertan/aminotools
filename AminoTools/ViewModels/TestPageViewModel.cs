using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Providers;
using AminoTools.ViewModels.Contracts;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class TestPageViewModel : BaseViewModel, ITestPageViewModel
    {
        private readonly IBlogProvider _blogProvider;
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

        public TestPageViewModel(IBlogProvider blogProvider)
        {
            _blogProvider = blogProvider;
            Initialize += TestPageViewModel_Initialize;
        }

        private void TestPageViewModel_Initialize(object sender, EventArgs e)
        {
            ButtonCommand = new Command(DoTest);  
        }

        private async void DoTest()
        {
            var blogs = await DoAsBusyState(_blogProvider.GetBlogsByUserIdAsync("x146561979", "9f8e3a79-03ca-4a25-bc95-3b257c765bad"));
        }
    }
}
