using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AminoTools.Converters
{
    public class DateTimeToNowRelatedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "n/a";
            var time = (DateTime) value;
            var timeDifference = DateTime.Now - time;
            if (timeDifference.Seconds <= 30)
            {
                return "a moment ago";
            }
            if (timeDifference.Hours == 0)
            {
                return $"{timeDifference.Minutes} minutes ago";
            }
            if (timeDifference.Days == 0)
            {
                return $"{timeDifference.Hours} hours ago";
            }
            if (timeDifference.Days <= 7)
            {
                return $"{timeDifference.Days} days ago";
            }
            return time.ToString("g");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
