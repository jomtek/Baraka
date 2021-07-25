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

namespace Baraka.Theme.UserControls.Quran.Player
{
    /// <summary>
    /// Logique d'interaction pour BarakaHizbVisualizer.xaml
    /// </summary>
    public partial class BarakaHizbVisualizer : UserControl
    {
        private int _surah;
        private int _totalVerses;

        private Tuple<Brush, Brush> _palette =
            new Tuple<Brush, Brush>(Brushes.BurlyWood, Brushes.Brown);

        public BarakaHizbVisualizer()
        {
            InitializeComponent();          
        }

        public void Prepare(int surah, int totalVerses)
        {
            _surah = surah;
            _totalVerses = totalVerses;
        }

        #region Load segments
        private Dictionary<int, HizbDescription> FindSegments()
        {
            var segments = new Dictionary<int, HizbDescription>(); // End verse: Hizb
            foreach (var hizbGroup in LoadedData.JuzAndHizb)
            {
                foreach (HizbDescription hizb in new[] { hizbGroup.Item1, hizbGroup.Item2 })
                {
                    if (hizb.StartSurah > _surah)
                    {
                        return segments;
                    }

                    if (hizb.StartSurah == _surah)
                    {
                        if (hizb.EndSurah != hizb.StartSurah)
                        {
                            segments.Add(_totalVerses, hizb);
                        }
                        else
                        {
                            segments.Add(hizb.EndVerse, hizb);
                        }
                    }
                    else if (hizb.EndSurah == _surah)
                    {
                        segments.Add(hizb.EndVerse, hizb);
                    }
                    else if (hizb.StartSurah < _surah && hizb.EndSurah > _surah)
                    {
                        // Surah is fully embed in the hizb
                        segments.Add(_totalVerses, hizb);
                    } 
                }
            }

            return segments;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            HizbSP.Children.Clear();

            var segments = FindSegments();

            int lastSeg = 0;
            foreach (KeyValuePair<int, HizbDescription> seg in segments)
            {
                int segLimit = seg.Key;
                var hizb = seg.Value;

                double ratio = (segLimit - lastSeg) / (double)_totalVerses;

                Brush fill = hizb.Number % 2 == 0 ? _palette.Item1 : _palette.Item2;

                var rect = new Rectangle()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Fill = fill,
                    Cursor = Cursors.Hand,
                    ToolTip = hizb.ToString(),
                    Height = double.NaN,
                };

                rect.Width = ContainerBorder.ActualWidth * ratio;

                HizbSP.Children.Add(rect);

                lastSeg = segLimit;
            }

            Console.WriteLine("test");
        }
        #endregion
    }
}
