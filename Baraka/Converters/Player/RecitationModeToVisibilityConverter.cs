using Baraka.Models.Quran;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Baraka.Singletons;
using System.Windows;

namespace Baraka.Converters.Player
{
    public class RecitationModeToVisbilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string mode)
            {
                if (mode == "")
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
