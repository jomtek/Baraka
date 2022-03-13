using Baraka.Models.Quran.Mushaf;
using Baraka.Services.Quran.Mushaf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Baraka.Converters.MushafDisplayer
{
    public class MushafGlyphTypeToFontUriConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is MushafGlyphType type && values[1] is int page)
            {
                switch (type)
                {
                    case MushafGlyphType.BASMALA:
                    case MushafGlyphType.SURA_NAME:
                        return new Uri(MushafFontService.FindPageFontName(0, true));
                    default:
                        return new Uri(MushafFontService.FindPageFontName(page, true));
                }
            }

            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
