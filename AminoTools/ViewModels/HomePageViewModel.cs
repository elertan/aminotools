using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models;
using AminoApi.Models.Blog;
using AminoApi.Models.Feed;
using AminoTools.Pages;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class HomePageViewModel : BaseViewModel, IHomePageViewModel
    {
        private readonly IFeedProvider _feedProvider;
        private IEnumerable<HeadLineBlog> _blogs;

        public HomePageViewModel(IFeedProvider feedProvider)
        {
            _feedProvider = feedProvider;
            Initialize += HomePageViewModel_Initialize;
        }

        public IEnumerable<HeadLineBlog> Blogs
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
            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Loading Feed";
                var feedHeadlinesResult = await _feedProvider.GetFeedHeadlines();

                if (!feedHeadlinesResult.DidSucceed())
                {
                    await Page.DisplayAlert("Something went wrong", feedHeadlinesResult.Info.Message, "Ok");
                    return;
                }

                Blogs = feedHeadlinesResult.Data.Blogs;
            });
        }
    }
}
