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
using System.Windows.Media.Animation;

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    public class DownloadRecitationEventArgs
    {
        public int Begin { get; set; }
        public int End { get; set; }

        public DownloadRecitationEventArgs(int begin, int end)
        {
            Begin = begin;
            End = end;
        }
    }

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
        public event EventHandler<DownloadRecitationEventArgs> DownloadRecitationRequested; // Beginning, End

        [Category("Baraka")]
        public event EventHandler<int> LoopModeDisplayed;
        #endregion

        public BarakaSurahDisplayer()
        {
            InitializeComponent();
            
            _relativeBmHeights = new List<double>();

            Bookmark.Displayer = this;
        }

        #region Prepare
        private double _topMargin = 2.5; // Space between verse-boxes

        private BarakaVerse GetVerseBoxFromSP(int index)
        {
            return VersesSP.Children[index] as BarakaVerse;
        }

        // This method is called whenever the last verse has finished loading its layout.
        // What it does:
        // - Generates the respective "verse numbers"
        // - Configures bookmark information
        // - Loads the last bookmark
        private void LastVerseBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            NumberingSP.Children.Clear();
            ReinitBookmark();


            double numIncrMargin = 0;
            double cumulatedHeights = 60;

            // Basmala
            if (Surah.HasBasmala())
            {
                var verseNum = new BarakaVerseNumber(this, -1, -1, true)
                {
                    Margin = new Thickness(0, 45, 0, 0)
                };

                NumberingSP.Children.Add(verseNum);

                numIncrMargin = GetVerseBoxFromSP(0).ActualHeight - 60 + _topMargin;

                _relativeBmHeights.Add(cumulatedHeights);
                cumulatedHeights += GetVerseBoxFromSP(0).ActualHeight + _topMargin;
            }

            // Verses
            for (int i = 0; i < Surah.NumberOfVerses; i++)
            {
                var verseNum = new BarakaVerseNumber(this, Surah.HasBasmala() ? i + 1 : i, i + 1);
                verseNum.Margin = new Thickness(0, numIncrMargin, 0, 0);

                if (i == 0 && !Surah.HasBasmala())
                {
                    verseNum.Margin = new Thickness(0, 45, 0, 0);
                }

                NumberingSP.Children.Add(verseNum);

                numIncrMargin = GetVerseBoxFromSP(verseNum.Number).ActualHeight - 60 + _topMargin;

                _relativeBmHeights.Add(cumulatedHeights);
                cumulatedHeights += GetVerseBoxFromSP(verseNum.Number).ActualHeight + _topMargin;
            }

            // Bookmark
            LoadLastBookmark();
        }

        public void LoadSurah(SurahDescription surah)
        {
            if (surah == Surah)
            {
                // The surah is already loaded
                return;
            }

            Surah = surah;
            StartVerse = 0;

            using (new Utils.WaitCursor())
            {
                ScrollToTop();
                VersesSP.Children.Clear();

                // Basmala
                if (surah.HasBasmala())
                {
                    var verseBox = new BarakaVerse(LoadedData.SurahList.ElementAt(0).Key, 0)
                    {
                        Margin = new Thickness(0, 45, 0, 0)
                    };
                    verseBox.Initialize();

                    VersesSP.Children.Add(verseBox);
                }

                // Verses
                for (int i = 0; i < surah.NumberOfVerses; i++)
                {
                    // Verse box
                    var verseBox = new BarakaVerse(surah, i);
                    verseBox.Initialize();

                    if (!(i == 0 && !Surah.HasBasmala())) // TODO: perhaps simplify this condition?
                    {
                        verseBox.Margin = new Thickness(0, _topMargin, 0, 0);
                    }

                    if (i + 1 == surah.NumberOfVerses)
                    {
                        // Last verse
                        verseBox.Margin = new Thickness(0, _topMargin, 0, 135);
                    }

                    if (i == 0 && !Surah.HasBasmala())
                    {
                        verseBox.Margin = new Thickness(0, 45, 0, 0);
                    }

                    // //

                    VersesSP.Children.Add(verseBox);

                    if (i == surah.NumberOfVerses - 1)
                    {
                        verseBox.SizeChanged += LastVerseBox_SizeChanged;
                    }

                    // Communicate the progress to the Welcome window
                    ((MainWindow)App.Current.MainWindow).ReportLoadingProgress((i / (double)surah.NumberOfVerses));
                }
            }

            MainSB.TargetValue = surah.NumberOfVerses;
        }


        #endregion

        public void LoadNextSurah()
        {
            // -1 is not required here because we want the NEXT surah
            if (Surah.SurahNumber < 114)
            {
                // Load next surah
                var nextSurah = LoadedData.SurahList.Keys.ElementAt(Surah.SurahNumber);
                LoadSurah(nextSurah);

                // Scroll to first verse
                VerseChanged?.Invoke(this, 0);
                ScrollToVerse(0);
            }
        }

        #region VerseNum Menu
        public void VerseNum_Click(int num)
        {
            if (num == -1) num = 0; // TODO: fix this mess
            VerseChanged?.Invoke(this, num);
            BrowseToVerse(num);
        }

        // TODO: temp
        public void DownloadRecitation(int beginningVerse = -1, int endVerse = -1)
        {
            if (beginningVerse == -1) beginningVerse = StartVerse;
            if (endVerse == -1) endVerse = EndVerse;

            // Managed in MainWindow.cs
            DownloadRecitationRequested?.Invoke(
                this,
                new DownloadRecitationEventArgs(beginningVerse, endVerse)
            );
        }

        public (int, int) GetNonRelativeBookmark()
        {
            return (0, 0);
        }
        #endregion

        #region Scroll
        public void ScrollToTop()
        {
            MainSB.ResetThumbY();
            VersesSV.ScrollToTop();
        }

        // SOF 2176945/anatoliy-nikolaev
        private void DoSmoothScoll(double verticalOffset)
        {
            if (VersesSV.VerticalOffset == verticalOffset)
            {
                return;
            }

            Console.WriteLine($"from: {VersesSV.VerticalOffset}, to: {verticalOffset}");

            DoubleAnimation verticalAnimation = new DoubleAnimation();

            verticalAnimation.From = VersesSV.VerticalOffset;
            verticalAnimation.To = verticalOffset;
            verticalAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(800));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(verticalAnimation);
            Storyboard.SetTarget(verticalAnimation, VersesSV);
            Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(Utils.UI.ScrollAnimationBehavior.VerticalOffsetProperty)); // Attached dependency property
            storyboard.Begin();
        }

        public void ScrollToVerse(int verse, bool searchRes = false)
        {
            Console.WriteLine($"scrolltoverse: {verse}");
            BrowseToVerse(verse);

            double newVerticalOffset = Bookmark.Height - 60 - 250;
            
            if (StartVerse != 0)
            {
                newVerticalOffset = Math.Abs(newVerticalOffset);
                newVerticalOffset += VersesSV.VerticalOffset;
            }

            if (newVerticalOffset > VersesSV.VerticalOffset)
            {
                if (VersesSV.ScrollableHeight != 0)
                {
                    // TODO: fix this
                    MainSB.Scrolled = newVerticalOffset / VersesSV.ScrollableHeight;
                }
            }
            else if (newVerticalOffset < VersesSV.VerticalOffset)
            {
                // Backwards scroll
                VersesSV.ScrollToVerticalOffset(0);
            }



            DoSmoothScoll(newVerticalOffset);

            if (searchRes)
            {
                var numPolygon = (BarakaVerseNumber)NumberingSP.Children[verse];
                numPolygon.MarkAsSearchResult();
            }
        }

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
            else
            {
                // Cannot use this anywhere
                MainSB.Scrolled = VersesSV.VerticalOffset / VersesSV.ScrollableHeight;
            }
        }
        #endregion
        #endregion

        #region Bookmark        
        #region General
        private void ReinitBookmark()
        {
            _relativeBmHeights.Clear();
            Bookmark.Height = 60;
            Bookmark.Margin = new Thickness(0, 45, 0, 0);
        }

        private void LoadLastBookmark()
        {
            int bookmark =
                LoadedData.Bookmarks[Surah.SurahNumber - 1];
            VerseChanged?.Invoke(this, bookmark);
            ScrollToVerse(bookmark);
        }
        #endregion

        #region Movement
        public void BrowseToVerse(int num)
        {
            EndVerse = num;

            if (StartVerse != 0 && num < StartVerse)
            {
                StartFromVerse(num);
            }

            if (num == -1) // Basmala support
            {
                Bookmark.Height = _relativeBmHeights[Math.Abs(num)];
            }
            else
            {
                Bookmark.Height = _relativeBmHeights[num];
            }

            if (StartVerse != 0)
            {
                Bookmark.Height -= _firstVerseOffset;
            }
            ActualVerse = num;

            // Save bookmark
            LoadedData.Bookmarks[Surah.SurahNumber - 1] = num;
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
        #endregion

        #region Loop Mode
        public void SetLoopMode(bool loop)
        {
            LoopMode = loop;
            Bookmark.SetLoopMode(loop);
            LoopModeDisplayed?.Invoke(this, StartVerse);
        }
        #endregion

        #region External Events
        public void ChangeVerse(int num, bool automatic = false)
        {
            if (_playing)
            {
                foreach (BarakaVerseNumber number in NumberingSP.Children)
                {
                    number.Playing = false;
                }

                var numPolygon = (BarakaVerseNumber)NumberingSP.Children[num];
                numPolygon.Playing = true;

                if (!LoopMode && LoadedData.Settings.AutoScrollQuran)
                    ScrollToVerse(num);
            }

            if (!LoopMode)
            {
                EndVerse = num;
            }
        }
        #endregion

        #region External UI configuration
        public void SetSBVisible(bool value)
        {
            if (value)
            {
                MainSB.Visibility = Visibility.Visible;
            }
            else
            {
                MainSB.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("loaded");
        }
    }
}
