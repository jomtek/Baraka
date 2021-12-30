using Baraka.Models.Quran;
using Baraka.Singletons.Streaming;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Baraka.Converters.TextDisplayer.Bookmark
{
    public class IsOutspreadVerseToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 &&
                values[0] is int startVerse &&
                values[1] is int endVerse &&
                values[2] is TextualVerseModel verse)
            {
                if (verse.Number >= startVerse && verse.Number < endVerse)
                {
                    return true;
                }
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
