using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Blog;
using AminoTools.DependencyServices;
using AminoTools.Models.Common.ImageSelection;
using AminoTools.Pages.Blogs;
using AminoTools.Pages.Common;
using AminoTools.ViewModels.Common;
using AminoTools.ViewModels.Contracts.Blogs;
using AminoTools.ViewModels.Contracts.Common;
using Autofac;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Blogs
{
    public class MultiBlogPageViewModel : BaseViewModel, IMultiBlogPageViewModel
    {
        private Blog _blog;
        private Command _nextCommand;
        private Command _imagesCommand;
        private ObservableCollection<BlogImageSource> _blogImageSources;

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

        public ObservableCollection<BlogImageSource> BlogImageSources
        {
            get => _blogImageSources;
            set
            {
                _blogImageSources = value; 
                OnPropertyChanged();
            }
        }

        public MultiBlogPageViewModel()
        {
            Blog = new Blog();
            ImagesCommand = new Command(DoSelectImages);
            NextCommand = new Command(DoNext);

            BlogImageSources = new ObservableCollection<BlogImageSource>();
            App.Variables.ImageSelection.UpdatedImages += ImageSelection_UpdatedImages;
        }

        private void ImageSelection_UpdatedImages(object sender, EventArgs e)
        {
            BlogImageSources = App.Variables.ImageSelection.BlogImageSources;
        }

        private async void DoSelectImages()
        {
            App.Variables.ImageSelection.BlogImageSources = BlogImageSources;

            var imageSelectionPage = new ImageSelectionPage();
            await App.MainNavigation.PushAsync(imageSelectionPage);
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
            App.Variables.MultiBlog.BlogImageSources = new List<BlogImageSource>(BlogImageSources);
            await App.MainNavigation.PushAsync(new CommunitySelectionPage());
        }

        private bool MayGoNext()
        {
            return !string.IsNullOrWhiteSpace(Blog.Title)
                   && !string.IsNullOrWhiteSpace(Blog.Content);
        }
    }
}
