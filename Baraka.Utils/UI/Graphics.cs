using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Baraka.Utils.UI
{
    public static class Graphics
    {
        public static double MeasureText(string text, TextBlock refTB)
        {
            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(refTB.FontFamily, refTB.FontStyle, refTB.FontWeight, refTB.FontStretch),
                refTB.FontSize,
                Brushes.Black,
                new NumberSubstitution(),
                1);

            return formattedText.Width;
        }
    }
}
