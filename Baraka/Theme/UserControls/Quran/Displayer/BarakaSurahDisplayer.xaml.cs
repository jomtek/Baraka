using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Streaming;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Baraka.Utils;

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaSurahDisplayer.xaml
    /// </summary>
    public partial class BarakaSurahDisplayer : UserControl
    {
        private bool _playing = false;
        private List<double> _relativeBmHeights;

        private SurahDescription _loadedSurah;

        #region Settings
        public bool Playing
        {
            get { return _playing; }
            set
            {
                _playing = value;

                if (value)
                {
                    Bookmark.Opacity = 1;
                }
                else
                {
                    Bookmark.Opacity = 0.35;
                }
            }
        }
        #endregion

        #region Events
        [Category("Baraka")]
        public event EventHandler<int> VerseChanged;

        [Category("Baraka")]
        public event EventHandler<int> DownloadVerseRequested;
        #endregion

        public BarakaSurahDisplayer()
        {
            InitializeComponent();
            _relativeBmHeights = new List<double>();
        }

        public void ScrollToTop()
        {
            MainSB.ResetThumbY();
            VersesSV.ScrollToTop();
        }

        public void ScrollToVerse(int verse)
        {
            BrowseToVerse(verse);
            VersesSV.ScrollToVerticalOffset(Bookmark.Height - 60);
        }

        #region Verse numbers click
        public void VerseNum_Click(int num)
        {
            VerseChanged?.Invoke(this, num);
            BrowseToVerse(num);
        }

        //temp
        public void DownloadMp3Verse(int verseNum)
        {
            DownloadVerseRequested?.Invoke(this, verseNum);
        }
        #endregion

        public void BrowseToVerse(int num)
        {
            if (_loadedSurah.SurahNumber != 1 && _loadedSurah.SurahNumber != 9)
            {
                Bookmark.Height = _relativeBmHeights[num];
            }
            else
            {
                Bookmark.Height = _relativeBmHeights[num - 1];
            }

            // Auto scroll (optional)
            //VersesSV.ScrollToVerticalOffset(Bookmark.Height - 60);
        }

        private void ReinitBookmark()
        {
            _relativeBmHeights.Clear();
            Bookmark.Height = 60;
        }

        public void WidthGo()
        {
            Console.WriteLine($"versessp width: {VersesSP.ActualWidth}");
        }

        public void LoadSurah(SurahDescription surah)
        {
            using (new Utils.WaitCursor())
            {
                ScrollToTop();

                VersesSP.Children.Clear();
                NumberingSP.Children.Clear();

                // Bookmark reinitialization
                ReinitBookmark();

                // //

                double topMargin = 3.5; // Top margin between verse-boxes

                double numIncrMargin = 0;
                double cumulatedHeights = 60;

                // Basmala
                if (surah.SurahNumber != 1 && surah.SurahNumber != 9) // Exclude Al-Fatiha and At-Tawbah
                {
                    var verseBox = new BarakaVerse(LoadedData.SurahList.ElementAt(0).Key, 0);
                    verseBox.Margin = new Thickness(0, 45, 0, 0);
                    verseBox.Initialize();

                    var verseNum = new BarakaVerseNumber(this, 0, true);
                    verseNum.Margin = new Thickness(0, 45, 0, 0);

                    VersesSP.Children.Add(verseBox);
                    NumberingSP.Children.Add(verseNum);

                    numIncrMargin = verseBox.ActualHeight - 60 + topMargin;

                    _relativeBmHeights.Add(cumulatedHeights);
                    cumulatedHeights += verseBox.ActualHeight + topMargin;
                }

                // Verses
                for (int i = 0; i < surah.NumberOfVerses; i++)
                {
                    // Verse box
                    var verseBox = new BarakaVerse(surah, i);
                    verseBox.Initialize();

                    if (!(i == 0 && (surah.SurahNumber == 1 || surah.SurahNumber == 9)))
                    {
                        verseBox.Margin = new Thickness(0, topMargin, 0, 0);
                    }

                    if (i + 1 == surah.NumberOfVerses)
                    {
                        verseBox.Margin = new Thickness(0, topMargin, 0, 130);
                    }

                    // Verse number
                    var verseNum = new BarakaVerseNumber(this, i + 1);
                    verseNum.Margin = new Thickness(0, numIncrMargin, 0, 0);

                    if (i == 0 && (surah.SurahNumber == 1 || surah.SurahNumber == 9))
                    {
                        verseBox.Margin = new Thickness(0, 45, 0, 0);
                        verseNum.Margin = new Thickness(0, 45, 0, 0);
                    }

                    // //

                    VersesSP.Children.Add(verseBox);
                    NumberingSP.Children.Add(verseNum);

                    numIncrMargin = verseBox.ActualHeight - 60 + topMargin;

                    _relativeBmHeights.Add(cumulatedHeights);
                    cumulatedHeights += verseBox.ActualHeight + topMargin;
                }

                _loadedSurah = surah;
            }

            MainSB.TargetValue = surah.NumberOfVerses;
        }

        private void MainSB_OnScroll(object sender, EventArgs e)
        {
            if (VersesSV.ScrollableHeight * MainSB.Scrolled > 0)
            {
                VersesSV.ScrollToVerticalOffset(VersesSV.ScrollableHeight * MainSB.Scrolled);
            }
        }

        private void VersesSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MainSB.Scrolled = VersesSV.VerticalOffset / VersesSV.ScrollableHeight;
        }
    }
}
