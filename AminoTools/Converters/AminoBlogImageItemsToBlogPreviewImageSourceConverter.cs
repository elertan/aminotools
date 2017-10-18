using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Media;
using Xamarin.Forms;

namespace AminoTools.Converters
{
    public class AminoBlogImageItemsToBlogPreviewImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageItems = (IEnumerable<ImageItem>) value;
            var uriImageSource = new UriImageSource();

            if (imageItems == null) return uriImageSource;

            var imageItem = imageItems.FirstOrDefault();
            if (imageItem != null)
            {
                uriImageSource.Uri = new Uri(imageItem.ImageUrl);
            }

            return uriImageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
