using System;
using System.Globalization;
using Xamarin.Forms;

namespace AminoTools.Converters
{
    public class StringIsNullOrWhiteSpaceToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (string) value;
            return !string.IsNullOrWhiteSpace(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
