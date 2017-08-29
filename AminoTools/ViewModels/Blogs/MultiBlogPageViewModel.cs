using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Blog;
using AminoTools.Pages.Blogs;
using AminoTools.ViewModels.Contracts.Blogs;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Blogs
{
    public class MultiBlogPageViewModel : BaseViewModel, IMultiBlogPageViewModel
    {
        private Blog _blog;
        private Command _nextCommand;

        public Blog Blog
        {
            get => _blog;
            set
            {
                _blog = value; 
                OnPropertyChanged();
            }
        }

        public Command NextCommand
        {
            get => _nextCommand;
            set
            {
                _nextCommand = value; 
                OnPropertyChanged();
            }
        }

        public MultiBlogPageViewModel()
        {
            Blog = new Blog();
            NextCommand = new Command(DoNext);
        }

        private async void DoNext()
        {
            // Show community selection screen
            if (!MayGoNext())
            {
                await Page.DisplayAlert("Oops!", "You haven't filled out the required information yet", "Ok");
                return;
            }

            // Store global variables
            App.Variables.MultiBlog.Blog = Blog;
            await App.MainNavigation.PushAsync(new CommunitySelectionPage());
        }

        private bool MayGoNext()
        {
            return !string.IsNullOrWhiteSpace(Blog.Title)
                   && !string.IsNullOrWhiteSpace(Blog.Content);
        }
    }
}
