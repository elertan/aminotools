﻿using System;
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
            var timeDifference = DateTime.UtcNow - time;
            if (timeDifference.TotalSeconds <= 60)
            {
                return "a moment ago";
            }
            if (timeDifference.TotalHours < 1)
            {
                return $"{timeDifference.Minutes} minute{BoolToPluralCharConverter(timeDifference.Minutes > 1)} ago";
            }
            if (timeDifference.TotalDays < 1)
            {
                return $"{timeDifference.Hours} hour{BoolToPluralCharConverter(timeDifference.Hours > 1)} ago";
            }
            if (timeDifference.TotalDays < 7)
            {
                return $"{timeDifference.Days} day{BoolToPluralCharConverter(timeDifference.Days > 1)} ago";
            }
            return time.ToString("d");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string BoolToPluralCharConverter(bool b) => b ? "s" : "";
    }
}
