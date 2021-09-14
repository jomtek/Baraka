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
using Baraka.Theme.UserControls.Quran.Display.Mushaf;
using System.Net;
using System.Diagnostics;
using Baraka.Theme.UserControls.Quran.Display.Mushaf.Data;

namespace Baraka.Theme.UserControls.Quran.Display.Translated
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
    /// Logique d'interaction pour BarakaTranslatedSurahDisplayer.xaml
    /// </summary>
    public partial class BarakaTranslatedSurahDisplayer : UserControl, ISurahDisplayer
    {
        private bool _initialized = false;
        
        #region Generation detail
        private List<double> _relativeBmHeights;
        private double _firstVerseOffset;
        #endregion
        
        #region Other detail
        private Storyboard _smoothScrollStory;
        #endregion

        #region Info
        private bool _playing = false;
        public bool LoopMode { get; private set; } = false;

        public SurahDescription Surah { get; set; }

        // Non relative (as always)
        public int ActualVerse { get; private set; } = 0;
        public int StartVerse { get; private set; } = 0;
        public int EndVerse { get; private set; } = 0;
        #endregion

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

        [Category("Baraka")]
        public event EventHandler EnabledChanged;
        #endregion

        public BarakaTranslatedSurahDisplayer()
        {
            InitializeComponent();
            
            _relativeBmHeights = new List<double>();
            _smoothScrollStory = new Storyboard();

            Bookmark.Displayer = this;
        }

        #region Prepare or unload
        private double _topMargin = 2.5; // Space between verse-boxes

        private BarakaVerse GetVerseBoxFromSP(int index)
        {
            return VersesSP.Children[index] as BarakaVerse;
        }

        // This handler is called whenever the last verse has finished loading its layout.
        // What it does:
        // - Generate the respective "verse numbers"
        // - Configure bookmark information
        // - Load the last bookmark (unless the Event args specify otherwise)
        private async void LastVerseBox_CompletedInitialize(object sender, bool loadLastBookmark)
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

            IsEnabled = true;
            if (_initialized)
            {
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                _initialized = true;
            }

            if (loadLastBookmark)
            {
                await LoadLastBookmarkAsync();
            }
        }

        // Set it to true, the verse displaying process stops
        private bool _cancellationToken = false;

        public async Task LoadSurahAsync(SurahDescription surah)
        {
            await LoadSurahAsync(surah, false, true);
        }

        public async Task LoadSurahAsync(SurahDescription surah, bool reload, bool loadLastBookmark)
        {

            if (surah == Surah && !reload)
            {
                // The surah is already loaded
                return;
            }

            _cancellationToken = true;
            await Task.Delay(100);
            _cancellationToken = false;

            IsEnabled = false;
            if (_initialized) EnabledChanged?.Invoke(this, EventArgs.Empty);

            Surah = surah;
            StartVerse = 0;
            
            using (new Utils.WaitCursor())
            {
                ScrollToTop();
                VersesSP.Children.Clear();

                // Basmala
                if (surah.HasBasmala())
                {
                    var verseBox = new BarakaVerse(Utils.Quran.General.FindSurah(1), 0)
                    {
                        Margin = new Thickness(0, 45, 0, 0)
                    };
                    verseBox.Initialize();

                    if (LoadedData.Settings.SurahVersionConfig.DisplayArabic)
                    {
                        // Use the first verse from Al-Fatiha (i.e. basmala)
                        List<MushafGlyphDescription> basmalaGlyphs = null;
                        foreach (var verse in LoadedData.MushafGlyphProvider.RetrieveSurah(1))
                        {
                            basmalaGlyphs = verse;
                            break;
                        }

                        verseBox.LoadArabicVersion(
                            LoadedData.MushafFontManager.FindPageFontFamily(1),
                            basmalaGlyphs,
                            1
                        );
                    }

                    VersesSP.Children.Add(verseBox);
                }

                // Load verses
                var mushafData = LoadedData.MushafGlyphProvider.RetrieveSurah(surah.SurahNumber);
                for (int i = 0; i < surah.NumberOfVerses; i++)
                {
                    // Verse box
                    var verseBox = new BarakaVerse(surah, i, loadLastBookmark);
                    verseBox.Initialize();
                    
                    // Fill in the Mushaf data for current verse (if necessary)
                    if (LoadedData.Settings.SurahVersionConfig.DisplayArabic)
                    {
                        List<MushafGlyphDescription> glyphs = mushafData.ElementAt(i);

                        FontFamily family = LoadedData.MushafFontManager.FindPageFontFamily(glyphs[0].Page);
                        verseBox.LoadArabicVersion(family, glyphs, glyphs[0].Page);
                    }

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
                        verseBox.CompletedInitialize += LastVerseBox_CompletedInitialize;
                    }

                    if (!_initialized)
                    {
                        // Communicate the progress to the Welcome window
                        ((MainWindow)App.Current.MainWindow).ReportLoadingProgress(i / ((double)surah.NumberOfVerses-1));
                    }
                    else
                    {
                        // Check cancellation token
                        if (_cancellationToken)
                        {
                            break;
                        }

                        await Task.Delay(1);
                    }
                }
            }

            MainSB.TargetValue = surah.NumberOfVerses;
        }

        public void UnloadActualSurah()
        {
            VersesSP.Children.Clear();
        }
        #endregion

        public async Task LoadNextSurahAsync()
        {
            // -1 is not required here because we want the NEXT surah
            if (Surah.SurahNumber != 114)
            {
                // Load next surah
                var nextSurah = LoadedData.SurahList.Keys.ElementAt(Surah.SurahNumber);
                await LoadSurahAsync(nextSurah, false, false);
            }
        }

        #region VerseNum Menu
        public async void VerseNum_Click(int num)
        {
            if (num == -1)
                num = 0; // TODO: fix this mess
            
            if (num != ActualVerse)
            {
                // Clear the highlightings of the actual verse before rushing to the desired one
                var actualVerseBox = VersesSP.Children[ActualVerse] as BarakaVerse;
                actualVerseBox.ClearHighlighting();
            }

            VerseChanged?.Invoke(this, num);
            await BrowseToVerseAsync(num);
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
        #region Utils
        // SOF 2176945/anatoliy-nikolaev
        private void DoSmoothScroll(double verticalOffset)
        {
            if (VersesSV.VerticalOffset == verticalOffset)
            {
                return;
            }

            DoubleAnimation verticalAnimation = new DoubleAnimation();

            verticalAnimation.From = VersesSV.VerticalOffset;
            verticalAnimation.To = verticalOffset;
            verticalAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(1200));

            _smoothScrollStory.Children.Clear();
            _smoothScrollStory.Children.Add(verticalAnimation);
            Storyboard.SetTarget(verticalAnimation, VersesSV);
            Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(Utils.UI.ScrollAnimationBehavior.VerticalOffsetProperty)); // Attached dependency property
            
            _smoothScrollStory.Begin();         
        }

        // Non-relative verse asked for
        private double GetTargetVerseOffset(int targetVerse)
        {
            return VersesSP.Children[targetVerse].TranslatePoint(new Point(), VersesSP).Y;
        }
        #endregion

        public void ScrollToTop()
        {
            MainSB.ResetThumbY();
            VersesSV.ScrollToTop();
        }

        public async Task ScrollToVerseAsync(int verse, bool searchRes = false)
        {
            await BrowseToVerseAsync(verse);
            double newVerticalOffset = GetTargetVerseOffset(verse) - 250;

            if (newVerticalOffset > VersesSV.VerticalOffset)
            {
                if (VersesSV.ScrollableHeight != 0)
                {
                    // TODO: fix this
                    MainSB.Scrolled = newVerticalOffset / VersesSV.ScrollableHeight;
                }
            }

            // Breathe (TODO)
            await Task.Delay(100);

            DoSmoothScroll(newVerticalOffset);

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

            // Pause auto-scroll when user interrupts it by manually scrolling
            if (_smoothScrollStory.GetCurrentState() == ClockState.Active)
            {
                _smoothScrollStory.Pause();
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

        private async Task LoadLastBookmarkAsync()
        {
            int bookmark =
                LoadedData.Bookmarks[Surah.SurahNumber - 1];
            VerseChanged?.Invoke(this, bookmark);
            await ScrollToVerseAsync(bookmark);
        }
        #endregion

        #region Movement
        public async Task BrowseToVerseAsync(int num)
        {
            EndVerse = num;

            if (StartVerse != 0 && num < StartVerse)
            {
                await StartFromVerseAsync(num);
            }

            if (num == -1) // Basmala support
            {
                Bookmark.Height = _relativeBmHeights[1];
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

        public async Task StartFromVerseAsync(int target)
        {
            StartVerse = target;

            VerseChanged?.Invoke(this, StartVerse);

            Bookmark.Margin = new Thickness(0, _relativeBmHeights[target] - 12, 0, 0);
            _firstVerseOffset = _relativeBmHeights[target] - 60;

            Bookmark.Height = 60; // Default; verse number size

            if (target < EndVerse)
            {
                await BrowseToVerseAsync(EndVerse);
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
        public async Task ChangeVerseAsync(int num, bool automatic = false)
        {
            if (_playing)
            {
                foreach (BarakaVerseNumber number in NumberingSP.Children)
                {
                    number.Playing = false;
                }

                var numPolygon = (BarakaVerseNumber)NumberingSP.Children[num];
                numPolygon.Playing = true;

                if (!LoopMode && LoadedData.Settings.AutoScrollQuran && automatic)
                {
                    await ScrollToVerseAsync(num);
                }
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
        }
    }
}
