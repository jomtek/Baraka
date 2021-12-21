using Baraka.Models.Quran;
using Baraka.Models.Quran.Mushaf;
using Baraka.Services.Quran.Mushaf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Baraka.Converters.TextDisplayer
{
    public class ArabicVerseToFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TextualVerseModel tvm && tvm.Arabic.IsActive)
            {
                // In the first two pages of the Mus'haf, the words are a bit smaller, so
                // we need to normalize their size using a specific formula.
                if (tvm.Arabic.Content[0].Page > 2)
                {
                    return 30;
                }
                else
                {
                    return 30 * (4 / 3d); // quran.com
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
