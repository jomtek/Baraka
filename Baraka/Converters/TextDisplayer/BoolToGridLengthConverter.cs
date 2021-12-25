using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Baraka.Converters.TextDisplayer
{
    public class BoolToGridLengthConverter : IValueConverter
    {
        // False gives 0, True gives Auto
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return GridLength.Auto;
            }
            else
            {
                return new GridLength(0);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}