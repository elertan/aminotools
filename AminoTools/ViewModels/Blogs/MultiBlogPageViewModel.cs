using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Blog;
using AminoTools.DependencyServices;
using AminoTools.Pages.Blogs;
using AminoTools.ViewModels.Contracts.Blogs;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Blogs
{
    public class MultiBlogPageViewModel : BaseViewModel, IMultiBlogPageViewModel
    {
        private Blog _blog;
        private Command _nextCommand;
        private Command _imagesCommand;
        private IEnumerable<Stream> _imageStreams;

        public IEnumerable<Stream> ImageStreams
        {
            get => _imageStreams;
            set
            {
                _imageStreams = value; 
                OnPropertyChanged();
            }
        }

        public Blog Blog
        {
            get => _blog;
            set
            {
                _blog = value; 
                OnPropertyChanged();
            }
        }

        public Command ImagesCommand
        {
            get => _imagesCommand;
            set
            {
                _imagesCommand = value; 
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
            ImagesCommand = new Command(DoSelectImages);
            NextCommand = new Command(DoNext);
        }

        private async void DoSelectImages()
        {
            var picturePicker = DependencyService.Get<IPicturePicker>();
            var stream = await picturePicker.GetImageStreamAsync();
            if (stream == null) return;


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
