using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
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
