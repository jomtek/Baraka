using Baraka.Models.Quran;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Baraka.Singletons;

namespace Baraka.Converters.Player
{
    public class SelectedQariToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is QariModel qari)
            {
                if (qari == AppStateSingleton.Instance.SelectedQariStore.Value)
                {
                    return Brushes.DarkGreen;
                }
            }

            return Brushes.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
