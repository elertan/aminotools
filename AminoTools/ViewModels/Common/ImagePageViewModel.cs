using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Pages.Common;
using AminoTools.ViewModels.Contracts.Common;

namespace AminoTools.ViewModels.Common
{
    public class ImagePageViewModel : BaseViewModel, IImagePageViewModel
    {
        private Uri _imageUri;

        public Uri ImageUri
        {
            get => _imageUri;
            set
            {
                _imageUri = value; 
                OnPropertyChanged();
            }
        }

        public ImagePageViewModel()
        {
            Initialize += ImagePageViewModel_Initialize;
        }

        private void ImagePageViewModel_Initialize(object sender, EventArgs e)
        {
            var imagePage = (ImagePage) Page;
            ImageUri = imagePage.ImageUri;
        }
    }
}
