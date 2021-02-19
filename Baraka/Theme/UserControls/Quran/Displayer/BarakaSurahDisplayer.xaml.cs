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
            LoadSurah(Data.LoadedData.SurahList.ElementAt(0).Key);
        }

        public void LoadSurah(SurahDescription surah)
        {
            VersesSP.Children.Clear();
            VersesSV.ScrollToTop();

            for (int i = 0; i < surah.NumberOfVerses; i++)
            {
                var verseBox = new BarakaVerse(surah, i);
                verseBox.Initialize();
                verseBox.Margin = new Thickness(0, 7.5, 0, 0);
                VersesSP.Children.Add(verseBox);
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
    }
}
