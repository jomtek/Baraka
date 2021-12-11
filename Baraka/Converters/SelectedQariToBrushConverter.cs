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
    public class SelectedQariToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is QariModel qari)
            {
                if (qari == AppStateSingleton.Instance.SelectedQari)
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
