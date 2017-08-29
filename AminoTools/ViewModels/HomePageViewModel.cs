using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models;
using AminoApi.Models.Blog;
using AminoTools.Pages;
using AminoTools.ViewModels.Contracts;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class HomePageViewModel : BaseViewModel, IHomePageViewModel
    {
        private Command _testButtonCommand;
        private IEnumerable<Blog> _blogs;

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

        public IEnumerable<Blog> Blogs
        {
            get => _blogs;
            set
            {
                _blogs = value; 
                OnPropertyChanged();
            }
        }

        private async void HomePageViewModel_Initialize(object sender, EventArgs e)
        {
            TestButtonCommand = new Command(DoNavigateToTestPage);

            var result = await DoAsBusyState(App.Api.S.GetBlogsByUserIdAsync("x146561979", "9f8e3a79-03ca-4a25-bc95-3b257c765bad"));
            Blogs = result.Data.Blogs;
        }

        private async void DoNavigateToTestPage()
        {
            await App.MainNavigation.PushAsync(new TestPage(), true);
        }
    }
}
