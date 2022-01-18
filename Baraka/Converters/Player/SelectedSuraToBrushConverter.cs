using Baraka.Models.Quran;
using Baraka.Models.State;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Baraka.Converters.Player
{
    public class SelectedSuraToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] is SuraModel sura && values[1] is AppState app)
            {
                if (sura == app.SelectedSuraStore.Value)
                {
                    return Brushes.DarkGreen;
                }
            }

            return Brushes.Black;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}