using Baraka.Models.Quran;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Baraka.Models.State;

namespace Baraka.Converters.Player
{
    public class SelectedQariToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] is QariModel qari && values[1] is AppState app)
            {
                if (qari == app.SelectedQariStore.Value)
                {
                    return Brushes.DarkGreen;
                }
            }

            return Brushes.LightGray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
