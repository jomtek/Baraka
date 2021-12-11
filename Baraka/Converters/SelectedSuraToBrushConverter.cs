using Baraka.Models.Quran;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Baraka.Singletons;
using System.Diagnostics;

namespace Baraka.Converters
{
    public class SelectedSuraToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SuraModel sura)
            {
                if (sura == AppStateSingleton.Instance.CurrentlyDisplayedSura)
                {
                    return Brushes.DarkGreen;
                }
            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
