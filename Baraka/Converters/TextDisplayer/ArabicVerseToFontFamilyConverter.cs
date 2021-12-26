using Baraka.Models.Quran;
using Baraka.Services.Quran.Mushaf;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Baraka.Converters.TextDisplayer
{
    public class ArabicVerseToFontFamilyConverter : IValueConverter
    {
        // Each glyph becomes an arabic word thanks to the FontFamily of the page it belongs to
        // This class simply gives a Verse its respective FontFamily
        // (!) Notice that, on the mus'haf, one verse never jumps from one page to another, so each verse
        //     clearly has one respective associated page
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
