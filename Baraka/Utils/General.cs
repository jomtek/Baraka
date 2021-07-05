using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Baraka.Utils
{
    public static class General
    {
        #region Graphics
        public static void BindWidth(this FrameworkElement bindMe, FrameworkElement toMe)
        {
            Binding b = new Binding();
            b.Mode = BindingMode.OneWay;
            b.Source = toMe.ActualWidth;
            bindMe.SetBinding(FrameworkElement.WidthProperty, b);
        }

        public static Point GetMousePositionWindowsForms()
        {
            var point = System.Windows.Forms.Control.MousePosition;
            return new Point(point.X, point.Y);
        }

        public static double GetFullScrollableHeight(ScrollViewer sv)
        {
            double oldVerticalOffset = sv.VerticalOffset;
            double scrollableHeight = 0;

            sv.ScrollToVerticalOffset(0);
            scrollableHeight = sv.ScrollableHeight;

            sv.ScrollToVerticalOffset(oldVerticalOffset);

            return scrollableHeight;
        }
        #endregion

        #region Quran
        public static bool CheckIfBasmala(SurahDescription surah)
        {
            // Exclude At-Tawba and Fatiha
            return surah.SurahNumber != 1 && surah.SurahNumber != 9;
        }
        #endregion

    }
}
