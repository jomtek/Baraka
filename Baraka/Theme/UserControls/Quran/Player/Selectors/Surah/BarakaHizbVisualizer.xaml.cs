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

namespace Baraka.Theme.UserControls.Quran.Player.Selectors.Surah
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
        private Dictionary<VerseDescription, HizbDescription> FindSegments()
        {
            var segments = new Dictionary<VerseDescription, HizbDescription>();

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
                                segments.Add(new VerseDescription(surah, surah.NumberOfVerses), hizb);
                            }
                            else
                            {
                                segments.Add(new VerseDescription(surah, hizb.EndVerse), hizb);
                            }
                        }
                        else if (hizb.EndSurah == surah.SurahNumber)
                        {
                            segments.Add(new VerseDescription(surah, hizb.EndVerse), hizb);
                        }
                        else if (hizb.StartSurah < surah.SurahNumber && hizb.EndSurah > surah.SurahNumber)
                        {
                            // Surah is fully embed in the hizb
                            segments.Add(new VerseDescription(surah, surah.NumberOfVerses), hizb);
                        }
                    }
                }
            }


            // Unify segments
            //
            var repetitiveSegments = new List<VerseDescription>();

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
            foreach (var seg in repetitiveSegments)
            {
                segments.Remove(seg);
            }

            // ---

            return segments;
        }  

        public void LoadSegments()
        {
            HizbSP.Children.Clear();

            var segments = FindSegments();

            double lastSegmentDue = 0;
            foreach (KeyValuePair<VerseDescription, HizbDescription> segDesc in segments)
            {              
                var segLimit = segDesc.Key;
                var hizb = segDesc.Value;

                double segmentHeight = -lastSegmentDue;

                // 52 is the height of a SurahBar
                segmentHeight += (segLimit.Surah.SurahNumber - 1) * 52;
                
                double surahCompletedRatio = segLimit.Number / (double)(Utils.Quran.General.FindSurah(segLimit.Surah.SurahNumber).NumberOfVerses);
                segmentHeight += surahCompletedRatio * 52;

                Brush bg = hizb.Number % 2 == 0 ? _palette.Item1 : _palette.Item2;
                var segment = new BarakaHizbSegment()
                {
                    ToolTip = hizb.ToString(),
                    Background = bg,
                    Width = ActualWidth,
                    Limit = segLimit
                };

                segment.PreviewMouseLeftButtonUp += (object _, MouseButtonEventArgs __) =>
                {
                    SetHizbSelected(true, hizb.Number - 1);

                    // TODO: why, when setting hizb.StartVerse instead of hizb.StartVerse-1,
                    //       am I targeting the wrong hizb ?
                    var surah = Utils.Quran.General.FindSurah(hizb.StartSurah);
                    Page.HizbSelected(new VerseDescription(surah, hizb.StartVerse-1));
                };

                segment.Height = segmentHeight;                
                HizbSP.Children.Add(segment);

                lastSegmentDue += segmentHeight;
            }
        }
        #endregion

        #region UI
        public void RefreshSelectedHizb(VerseDescription verse)
        {
            // Find segment index based on verse
            foreach (BarakaHizbSegment segment in HizbSP.Children)
            {
                if ((verse.Surah.SurahNumber < segment.Limit.Surah.SurahNumber) ||
                    (verse.Surah.SurahNumber == segment.Limit.Surah.SurahNumber) && verse.Number <= segment.Limit.Number)
                {
                    SetHizbSelected(true, segment);
                    break;
                }
            }
        }

        private void SetHizbSelected(bool selected, int segIndex)
        {
            var segment = HizbSP.Children[segIndex] as BarakaHizbSegment;
            SetHizbSelected(selected, segment);
        }

        private void SetHizbSelected(bool selected, BarakaHizbSegment segment)
        {
            // Unselect all segments
            foreach (BarakaHizbSegment seg in HizbSP.Children)
            {
                seg.Selected = false;
            }

            if (selected)
            {
                segment.Selected = true;
            }
        }
        #endregion
    }
}
