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
    public class ArabicVerseToGlyphInlinesConverter : IValueConverter
    {
        // This class generates arabic word inlines from a TextualVerseModel
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var inlines = new List<Inline>();
            if (value is TextualVerseModel tvm && tvm.Arabic.IsActive)
            {
                foreach (MushafGlyphModel glyph in tvm.Arabic.Content)
                {
                    var run = new Run(glyph.DecodedData.ToString());

                    switch (glyph.Type)
                    {
                        case MushafGlyphType.STOPPING_SIGN:
                            run.Foreground = Brushes.Red;
                            break;
                        case MushafGlyphType.SUJOOD:
                            run.ToolTip = "Prosternez-vous (sajada) lorsque vous rencontrez ce symbôle";
                            run.Cursor = Cursors.Hand;
                            break;
                        case MushafGlyphType.RUB_EL_HIZB:
                            run.ToolTip = "Délimitation d'un quart de hizb";
                            run.Cursor = Cursors.Hand;
                            break;
                        case MushafGlyphType.END_OF_AYAH:
                            run.ToolTip = $"Verset {glyph.AssociatedVerse.Number}, page n°{glyph.Page}";
                            run.Cursor = Cursors.Hand;
                            break;
                        default: // Word
                            break;
                    }

                    inlines.Add(run);
                }
            }

            return inlines;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
