using Baraka.Models.Quran.Mushaf;
using Baraka.Services.Quran.Mushaf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Baraka.Converters.MushafDisplayer
{
    public class MushafLineToSuraTransitionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MushafGlyphModel[] glyphs)
            {
                if (glyphs[0].Type == MushafGlyphType.SURA_NAME)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
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
