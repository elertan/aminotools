using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Core;
using Xamarin.Forms;

namespace AminoTools.Converters
{
    public class BoolToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) throw new NotSupportedException($"Either value or the parameter was not given on the {nameof(BoolToInvertedBoolConverter)}");

            var b = (bool) value;

            if (b) return parameter;

            return Activator.CreateInstance(targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
