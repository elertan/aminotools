using System.IO;
using Xamarin.Forms;

namespace AminoTools.Models.Common.ImageSelection
{
    public class BlogImageSource : BaseModel
    {
        private MemoryStream _memoryStream;
        private ImageSource _imageSource;
        private string _blogReference;
        private string _description;

        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }    
        }
        

        public MemoryStream MemoryStream
        {
            get => _memoryStream;
            set
            {
                _memoryStream = value;
                OnPropertyChanged();

                var stream = new MemoryStream();
                MemoryStream.Position = 0;
                MemoryStream.CopyTo(stream);
                MemoryStream.Position = 0;

                ImageSource = ImageSource.FromStream(() => stream);
            }
        }

        public string BlogReference
        {
            get => _blogReference;
            set
            {
                _blogReference = value; 
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value; 
                OnPropertyChanged();
            }
        }
    }
}
