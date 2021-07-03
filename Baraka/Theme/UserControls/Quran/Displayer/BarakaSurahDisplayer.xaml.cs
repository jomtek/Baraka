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
using System.Windows.Media;
using System.Threading.Tasks;

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaSurahDisplayer.xaml
    /// </summary>
    public partial class BarakaSurahDisplayer : UserControl
    {
        private bool _playing = false;
        public bool LoopMode { get; private set; } = false;
        private List<double> _relativeBmHeights;

        public SurahDescription Surah { get; set; }

        // Non relative (as always)
        public int ActualVerse { get; private set; } = 0;
        public int StartVerse { get; private set; } = 0;
        public int EndVerse { get; private set; } = 0;

        //
        private double _firstVerseOffset;

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
                    Bookmark.Opacity = 0.50;
                    
                    foreach (BarakaVerseNumber number in NumberingSP.Children)
                    {
                        number.Playing = false;
                    }
                }
            }
        }
        #endregion

        #region Events
        [Category("Baraka")]
        public event EventHandler<int> VerseChanged;

        [Category("Baraka")]
        public event EventHandler<int> DownloadVerseRequested;

        [Category("Baraka")]
        public event EventHandler<int> LoopModeDisplayed;
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

        public void ScrollToVerse(int verse, bool searchRes = false)
        {
            BrowseToVerse(verse);
            VersesSV.ScrollToVerticalOffset(Bookmark.Height - 60);

            if (searchRes)
            {
                var numPolygon = (BarakaVerseNumber)NumberingSP.Children[verse];
                numPolygon.MarkAsSearchResult();
            }
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

        #region Bookmark movement

        public void BrowseToVerse(int num)
        {
            EndVerse = num;

            if (StartVerse != 0 && num < StartVerse)
            {
                StartFromVerse(num);
            }

            Bookmark.Height = _relativeBmHeights[num];
            if (StartVerse != 0)
            {
                Bookmark.Height -= _firstVerseOffset;
            }

            ActualVerse = num;

            // Auto scroll (optional)
            //VersesSV.ScrollToVerticalOffset(Bookmark.Height - 60);
        }

        public void StartFromVerse(int target)
        {
            StartVerse = target;
            
            VerseChanged?.Invoke(this, StartVerse);

            Bookmark.Margin = new Thickness(0, _relativeBmHeights[target] - 12, 0, 0);
            _firstVerseOffset = _relativeBmHeights[target] - 60;

            Bookmark.Height = 60; // Default; verse number size

            if (target < EndVerse)
            {
                BrowseToVerse(EndVerse);
                VerseChanged?.Invoke(this, StartVerse);
            }
        }
        #endregion

        #region Bookmark
        private void ReinitBookmark()
        {
            _relativeBmHeights.Clear();
            Bookmark.Height = 60;
            Bookmark.Margin = new Thickness(0, 45, 0, 0);
        }
        #endregion

        public void LoadSurah(SurahDescription surah)
        {
            Surah = surah;

            using (new Utils.WaitCursor())
            {
                ScrollToTop();

                VersesSP.Children.Clear();
                NumberingSP.Children.Clear();

                ReinitBookmark();

                StartVerse = 0;

                // //

                double topMargin = 2.5; // Space between verse-boxes

                double numIncrMargin = 0;
                double cumulatedHeights = 60;

                // Exclude Al-Fatiha and At-Tawbah
                bool basmala = surah.SurahNumber != 1 && surah.SurahNumber != 9;

                // Basmala
                if (basmala) 
                {
                    var verseBox = new BarakaVerse(LoadedData.SurahList.ElementAt(0).Key, 0);
                    verseBox.Margin = new Thickness(0, 45, 0, 0);
                    verseBox.Initialize();

                    var verseNum = new BarakaVerseNumber(this, -1, -1, true);
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
                        verseBox.Margin = new Thickness(0, topMargin, 0, 170);
                    }

                    // Verse number
                    var verseNum = new BarakaVerseNumber(this, basmala?i+1:i, i+1);
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
            }

            MainSB.TargetValue = surah.NumberOfVerses;
        }

        #region Loop
        public void SetLoopMode(bool loop)
        {
            LoopMode = loop;
            Bookmark.SetLoopMode(loop);
            LoopModeDisplayed?.Invoke(this, StartVerse);
        }
        #endregion

        #region Handlers
        private void MainSB_OnScroll(object sender, EventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control) return;
            if (VersesSV.ScrollableHeight * MainSB.Scrolled > 0)
            {
                VersesSV.ScrollToVerticalOffset(VersesSV.ScrollableHeight * MainSB.Scrolled);
            }
        }

        private void VersesSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                return;
            }
            MainSB.Scrolled = VersesSV.VerticalOffset / VersesSV.ScrollableHeight;
        }


        // Support for external handler (Player class)
        public void ChangeVerse(int num)
        {
            if (_playing)
            {
                foreach (BarakaVerseNumber number in NumberingSP.Children)
                {
                    number.Playing = false;
                }

                var numPolygon = (BarakaVerseNumber)NumberingSP.Children[num];
                numPolygon.Playing = true;
            }

            if (!LoopMode)
            {
                EndVerse = num;
            }
        }
        #endregion
    }
}
