using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AminoApi.Models.Blog;
using AminoApi.Models.Media;
using AminoTools.DependencyServices;
using AminoTools.Models.Common;
using AminoTools.Models.Common.ImageSelection;
using AminoTools.Pages.Blogs;
using AminoTools.Pages.Common;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Common;
using AminoTools.ViewModels.Contracts.Blogs;
using AminoTools.ViewModels.Contracts.Common;
using Autofac;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Blogs
{
    public class MultiBlogPageViewModel : BaseViewModel, IMultiBlogPageViewModel
    {
        private readonly IBlogProvider _blogProvider;
        private readonly IMediaProvider _mediaProvider;
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

        public MultiBlogPageViewModel(IBlogProvider blogProvider, IMediaProvider mediaProvider)
        {
            _blogProvider = blogProvider;
            _mediaProvider = mediaProvider;
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
            await App.MainNavigation.PushAsync(new CommunitySelectionPage(CommunitySelectionResultAction));
        }

        private async void CommunitySelectionResultAction(CommunitySelectionResult result)
        {
            if (result?.SelectedCommunities == null 
                || !result.SelectedCommunities.Any()) return;

            var cts = new CancellationTokenSource();
            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Getting Selected Communities";
                IsBusyData.IsProgessBarVisible = true;

                IsBusyData.Description = "Uploading Images";
                var imageItems = new List<ImageItem>();
                var imagesIterator = 0;
                foreach (var imageSource in App.Variables.MultiBlog.BlogImageSources)
                {
                    var imageItemResult = await _mediaProvider.UploadImage(imageSource.MemoryStream);

                    if (!imageItemResult.DidSucceed())
                    {
                        await Page.DisplayAlert("Image Upload Failed", imageItemResult.Info.Message, "Ok");
                        return;
                    }

                    imageItems.Add(imageItemResult.Data);
                    imagesIterator++;

                    IsBusyData.Progress = imagesIterator / (float)App.Variables.MultiBlog.BlogImageSources.Count;
                }

                IsBusyData.Progress = 0f;

                var blog = App.Variables.MultiBlog.Blog;
                var i = 0;
                foreach (var community in result.SelectedCommunities)
                {
                    IsBusyData.Description = $"Sending blog to {community.Name}";
                    if (i != 0) IsBusyData.Progress = i / (float)result.SelectedCommunities.Count;
                    await _blogProvider.PostBlog(community.Id, blog.Title, blog.Content, imageItems);
                    i++;
                    if (!cts.IsCancellationRequested) continue;

                    await Page.DisplayAlert("Canceled", $"Your blog has been posted on {i} communities, we can't change that.", "Ok");
                    return;
                }
                await Page.DisplayAlert("Succes!",
                    $"Your blog has been posted on {i} communities", "Ok");
            }, cts);


            await App.MainNavigation.PopToRootAsync();
        }

        private bool MayGoNext()
        {
            return !string.IsNullOrWhiteSpace(Blog.Title)
                   && !string.IsNullOrWhiteSpace(Blog.Content);
        }
    }
}
