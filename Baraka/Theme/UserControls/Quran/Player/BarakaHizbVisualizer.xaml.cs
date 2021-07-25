using Baraka.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Baraka.Data.Surah;
using Baraka.Data.Descriptions;

namespace Baraka.Theme.UserControls.Quran.Player
{
    /// <summary>
    /// Logique d'interaction pour BarakaHizbVisualizer.xaml
    /// </summary>
    public partial class BarakaHizbVisualizer : UserControl
    {
        public SurahSelectorPage Page { get; set; }
        private Tuple<Brush, Brush> _palette = new Tuple<Brush, Brush>(
            (SolidColorBrush)App.Current.Resources["DarkBrush"],
            (SolidColorBrush)App.Current.Resources["LightBrush"]
        );

        public BarakaHizbVisualizer()
        {
            InitializeComponent();
        }

        #region Load segments
        private Dictionary<(int, int), HizbDescription> FindSegments()
        {
            var segments = new Dictionary<(int, int), HizbDescription>(); // (Surah number, End verse): Hizb
            
            // Find segments
            //
            foreach (var hizbGroup in LoadedData.JuzAndHizb)
            {
                foreach (HizbDescription hizb in new[] { hizbGroup.Item1, hizbGroup.Item2 })
                {
                    foreach (SurahDescription surah in LoadedData.SurahList.Keys)
                    {
                        if (hizb.StartSurah > surah.SurahNumber)
                        {
                            continue;
                        }

                        if (hizb.StartSurah == surah.SurahNumber)
                        {
                            if (hizb.EndSurah != hizb.StartSurah)
                            {
                                segments.Add((surah.SurahNumber, surah.NumberOfVerses), hizb);
                            }
                            else
                            {
                                segments.Add((surah.SurahNumber, hizb.EndVerse), hizb);
                            }
                        }
                        else if (hizb.EndSurah == surah.SurahNumber)
                        {
                            segments.Add((surah.SurahNumber, hizb.EndVerse), hizb);
                        }
                        else if (hizb.StartSurah < surah.SurahNumber && hizb.EndSurah > surah.SurahNumber)
                        {
                            // Surah is fully embed in the hizb
                            segments.Add((surah.SurahNumber, surah.NumberOfVerses), hizb);
                        }
                    }
                }
            }


            // Unify segments
            //
            var repetitiveSegments = new List<(int, int)>();

            for (int i = 0; i < segments.Count; i++)
            {
                var segment = segments.ElementAt(i);

                // Lookahead
                int j = 0;
                while (true)
                {
                    if (segments.Count > i + j + 1)
                    {
                        var segmentAhead = segments.ElementAt(i + j + 1);
                        if (segmentAhead.Value.Number == segment.Value.Number)
                        {
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // Unify
                for (int k = 0; k < j; k++)
                {
                    repetitiveSegments.Add(segments.ElementAt(i + k).Key);
                }

                i += j;
            }

            // Remove repetitive occurrences
            foreach ((int, int) seg in repetitiveSegments)
            {
                segments.Remove(seg);
            }

            // ---

            return segments;
        }

        /* TODO
        private void SetHizbSelected(bool selected, Rectangle hizbRect)
        {
            if (selected)
            {
            }
        }
        */

        public void LoadSegments()
        {
            HizbSP.Children.Clear();

            var segments = FindSegments();

            double lastSegmentDue = 0;
            foreach (KeyValuePair<(int, int), HizbDescription> segDesc in segments)
            {              
                var hizb = segDesc.Value;

                double segmentHeight = -lastSegmentDue;

                int surahNum = segDesc.Key.Item1;
                int verseNum = segDesc.Key.Item2;

                // 52 is the height of a SurahBar
                segmentHeight += (surahNum - 1) * 52;

                double surahCompletedRatio = verseNum / (double)LoadedData.SurahList.ElementAt(surahNum - 1).Key.NumberOfVerses;
                segmentHeight += surahCompletedRatio * 52;

                Brush fill = hizb.Number % 2 == 0 ? _palette.Item1 : _palette.Item2;
                var segment = new Border()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Cursor = Cursors.Hand,
                    Background = fill,
                    BorderThickness = new Thickness(0, 0, 0, 5),
                    ToolTip = hizb.ToString(),
                    Width = ActualWidth
                };

                segment.MouseEnter += (object _, MouseEventArgs __) =>
                {
                    segment.BorderBrush = Brushes.Goldenrod;
                };

                segment.MouseLeave += (object _, MouseEventArgs __) =>
                {
                    segment.BorderBrush = Brushes.Transparent;
                };

                segment.PreviewMouseLeftButtonUp += (object _, MouseButtonEventArgs __) =>
                {
                    var surah = LoadedData.SurahList.ElementAt(hizb.StartSurah - 1).Key;
                    Page.HizbSelected(new VerseDescription(surah, hizb.StartVerse - 1));
                };

                segment.Height = segmentHeight;                
                HizbSP.Children.Add(segment);

                lastSegmentDue += segmentHeight;
            }
        }
        #endregion
    }
}
