using Baraka.Models.Quran;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Baraka.Models.State;

namespace Baraka.Converters.Player
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolean)
            {
                return boolean ? Brushes.Black : Brushes.DarkGreen;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
