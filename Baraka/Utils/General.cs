using Baraka.Data;
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
using System.Windows.Media;

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
        #endregion

        #region Quran
        public static string GenerateSynopsis(SurahDescription surah)
        {
            string revelationType;

            switch (surah.RevelationType)
            {
                case SurahRevelationType.M:
                    revelationType = "mecquoise (La Mecque)";
                    break;
                case SurahRevelationType.H:
                    revelationType = "médinoise (Médine)";
                    break;
                default:
                    revelationType = "mecquoise ou médinoise";
                    break;
            }

            return $"Contient {surah.NumberOfVerses} versets\nRévélation {revelationType}";
        }

        public static string PrettyPrintVerse(int verseNum, SurahDescription surah)
        {
            //if (verseNum == 0) verseNum = 1;

            var firstTransId =
                LoadedData.TranslationsList[LoadedData.Settings.SurahVersionConfig.Translation1].Identifier;
            var surahVer = LoadedData.SurahList[surah][firstTransId];
            string verse = surahVer.Verses[verseNum];

            if (surah.HasBasmala())
            {
                if (verseNum == 0)
                {
                    verse = LoadedData.SurahList.ElementAt(0).Value[firstTransId].Verses[0];
                }
                else
                {
                    verse = surahVer.Verses[verseNum-1];
                }
            }
            else
            {
                verseNum++;
            }

            return $"{surah.SurahNumber}:{verseNum} {verse}";
        }
        #endregion

        #region Security
        // SOF 132474/michael
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        #endregion
    }
}
