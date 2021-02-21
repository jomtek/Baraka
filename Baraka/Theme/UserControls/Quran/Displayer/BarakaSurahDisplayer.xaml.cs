using Baraka.Data.Descriptions;
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
using System.ComponentModel;

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaSurahDisplayer.xaml
    /// </summary>
    public partial class BarakaSurahDisplayer : UserControl
    {
        [Category("Baraka")]
        public event EventHandler OnPlanetClick;

        public BarakaSurahDisplayer()
        {
            InitializeComponent();
        }

        public void LoadSurah(SurahDescription surah)
        {
            VersesSP.Children.Clear();
            NumberingSP.Children.Clear();
            VersesSV.ScrollToTop();

            double numIncrementalMargin = 0;
            double lastVerseHeight = 0;

            for (int i = 0; i < surah.NumberOfVerses; i++)
            {
                var verseBox = new BarakaVerse(surah, i);
                verseBox.Initialize();
                if (lastVerseHeight != 0) verseBox.Margin = new Thickness(0, 7.5, 0, 0);
                
                var verseNum = new BarakaVerseNumber(i + 1);
                verseNum.Margin = new Thickness(0, numIncrementalMargin, 0, 0);

                VersesSP.Children.Add(verseBox);
                NumberingSP.Children.Add(verseNum);

                lastVerseHeight = verseBox.ActualHeight;
                numIncrementalMargin = lastVerseHeight - 60 + 7.5;
            }
        }

        private void MainSB_OnScroll(object sender, EventArgs e)
        {
            VersesSV.ScrollToVerticalOffset(VersesSV.ScrollableHeight * MainSB.Scrolled);
        }

        private void BarakaBookmark_OnPlanetClick(object sender, EventArgs e)
        {
            OnPlanetClick.Invoke(sender, e);
        }

        private void VersesSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MainSB.Scrolled = VersesSV.VerticalOffset / VersesSV.ScrollableHeight;
        }
    }
}
