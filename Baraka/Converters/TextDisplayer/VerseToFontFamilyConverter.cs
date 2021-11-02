using Baraka.Models.Quran;
using Baraka.Services.Quran.Mushaf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Baraka.Converters.TextDisplayer
{
    public class VerseToFontFamilyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TextualVerseModel tvm && tvm.Arabic.IsActive)
            {
                return MushafFontService.FindPageFontFamily(tvm.Arabic.Content[0].Page);
            }
            else
            {
                return new FontFamily("Arial");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
