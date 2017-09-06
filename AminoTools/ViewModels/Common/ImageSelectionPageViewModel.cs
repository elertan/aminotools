using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.DependencyServices;
using AminoTools.Models.Common.ImageSelection;
using AminoTools.ViewModels.Contracts.Common;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Common
{
    public class ImageSelectionPageViewModel : BaseViewModel, IImageSelectionPageViewModel
    {
        private ObservableCollection<BlogImageSource> _blogImageSources;
        public Command AddNewImageCommand { get; }
        public Command RemoveImageCommand { get; }

        public ObservableCollection<BlogImageSource> BlogImageSources
        {
            get => _blogImageSources;
            set
            {
                _blogImageSources = value; 
                OnPropertyChanged();
            }
        }

        public ImageSelectionPageViewModel()
        {
            AddNewImageCommand = new Command(DoAddNewImage);
            RemoveImageCommand = new Command(DoRemoveImage);
            BlogImageSources = App.Variables.ImageSelection.BlogImageSources ?? new ObservableCollection<BlogImageSource>();

            Initialize += ImageSelectionPageViewModel_Initialize;
        }

        private void Page_Disappearing(object sender, EventArgs e)
        {
            App.Variables.ImageSelection.BlogImageSources = BlogImageSources;
            App.Variables.ImageSelection.OnUpdatedImages();
        }

        private void ImageSelectionPageViewModel_Initialize(object sender, EventArgs e)
        {
            Page.Disappearing += Page_Disappearing;

            if (BlogImageSources == null) BlogImageSources = new ObservableCollection<BlogImageSource>();
        }

        private void DoRemoveImage(object o)
        {
            var blogImageSource = (BlogImageSource) o;
            BlogImageSources.Remove(blogImageSource);
        }

        private async void DoAddNewImage()
        {
            var picturePicker = DependencyService.Get<IPicturePicker>();
            using (var stream = await picturePicker.GetImageStreamAsync())
            {
                if (stream == null) return;

                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                var blogImageSource = new BlogImageSource {MemoryStream = memoryStream};

                BlogImageSources.Add(blogImageSource);
            }
        }
    }
}
