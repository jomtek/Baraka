using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Baraka.Converters.TextDisplayer.Bookmark
{
    public class BoolToVerticalAlignmentConverter : IValueConverter
    {
        // If the bookmark is outspread, its layout is stretched, whereas if it's not, its layout is set to top-align
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return VerticalAlignment.Stretch;
            }
            else
            {
                return VerticalAlignment.Top;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
