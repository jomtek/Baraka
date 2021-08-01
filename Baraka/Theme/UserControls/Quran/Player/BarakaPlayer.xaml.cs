using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Streaming;
using Baraka.Theme.UserControls.Quran.Displayer;
using Baraka.Theme.UserControls.Quran.Player.Selectors.Cheikh;
using Baraka.Theme.UserControls.Quran.Player.Selectors.Surah;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Baraka.Theme.UserControls.Quran.Player
{
    /// <summary>
    /// Logique d'interaction pour BarakaPlayer.xaml
    /// </summary>
    public partial class BarakaPlayer : UserControl
    {
        private bool _playing = false;
        private bool _loopMode = false;

        // UI relative
        private bool _cheikhModification = false;
        private bool _surahModification = false;
        private bool _wasPlaying = true;
        private bool _closing = false;
        private int _lastTabShown = -1;

        private CheikhDescription _selectedCheikh;
        private CheikhCard _selectedCheikhCard;

        private SurahDescription _selectedSurah;
        private SurahBar _selectedSurahBar;

        // Selectors
        private CheikhSelectorPage _cheikhSelector;
        private SurahSelectorPage _surahSelector;

        // Bindings
        public QuranStreamer Streamer { get; private set; }
        public BarakaSurahDisplayer Displayer { get; set; }

        #region Settings
        [Category("Baraka")]
        public bool Playing
        {
            get { return _playing; }
            set
            {
                if (value != _playing)
                {
                    _playing = value;

                    Streamer.Playing = value;
                    Displayer.Playing = value;
                    RefreshPlayPauseBtn();
                }
            }
        }

        [Category("Baraka")]
        public CheikhDescription SelectedCheikh
        {
            get { return _selectedCheikh; }
        }

        [Category("Baraka")]
        public SurahDescription SelectedSurah
        {
            get { return _selectedSurah; }
        }

        [Category("Baraka")]
        public VerseDescription SelectedVerse
        {
            get
            {
                return new VerseDescription(_selectedSurah, Streamer.Verse);
            }
        }
        #endregion

        public BarakaPlayer()
        {
            InitializeComponent();

            _selectedSurah = LoadedData.SurahList.ElementAt(0).Key;
            _selectedCheikh = LoadedData.CheikhList.ElementAt(0);

            SurahTB.ToolTip = Utils.General.GenerateSynopsis(_selectedSurah);

            // Selectors init
            _cheikhSelector = new CheikhSelectorPage();
            _surahSelector = new SurahSelectorPage();

            _surahSelector.HizbClicked += SurahSelector_HizbClicked;

            FrameComponent.Content = _cheikhSelector;

            // Streamer config
            Streamer = new QuranStreamer();
            Streamer.VerseChanged += Streamer_VerseChanged;
            Streamer.FinishedSurah += Streamer_FinishedSurah;
            Streamer.WordHighlightRequested += Streamer_WordHighlightRequested;

            // Stories
            ((Storyboard)this.Resources["PlayerCloseStory"]).Begin();
            ((Storyboard)this.Resources["PlayerCloseStory"]).SkipToFill();
            ((Storyboard)this.Resources["PlayerCloseStory"]).Completed += (object sender, EventArgs e) =>
            {
                SelectorGrid.Visibility = Visibility.Hidden;
                _closing = false;
                _surahSelector.ReloadList();
            };
        }

        public void Dispose()
        {
            Streamer.Playing = false;
        }

        public void ChangeVerse(int num)
        {
            Streamer.ChangeVerse(num);
        }

        #region Selector events
        private async void SurahSelector_HizbClicked(object sender, VerseDescription hizbStart)
        {
            await ((MainWindow)App.Current.MainWindow).IntersurahChangeVerseAsync(hizbStart, true);
        }
        #endregion

        #region Streamer events
        private async void Streamer_VerseChanged(object sender, EventArgs e)
        {
            if (Displayer.LoopMode)
            {
                ReinitLoopmodeInfo();
            }
            else
            {
                await Displayer.BrowseToVerseAsync(Streamer.NonRelativeVerse);
            }

            await Displayer.ChangeVerseAsync(Streamer.NonRelativeVerse, true);
        }

        private async void Streamer_FinishedSurah(object sender, EventArgs e)
        {
            _playing = false;
            RefreshPlayPauseBtn();

            if (LoadedData.Settings.AutoNextSurah)
            {
                await Displayer.LoadNextSurahAsync();
                await ChangeSelectedSurahAsync(Displayer.Surah, false);
                Playing = true;
            }
        }

        private void Streamer_WordHighlightRequested(object sender, int wIndex)
        {
            if (Displayer.ActualVerse >= Displayer.VersesSP.Children.Count)
            {
                // TODO: make this a little bit cleaner
                // Avoid OutOfRange when suddenly changing surah
                return;
            }

            var verse = Displayer.VersesSP.Children[Displayer.ActualVerse] as BarakaVerse;
            if (wIndex == -1)
            {
                verse.ClearHighlighting();
            }
            else
            {
                verse.HighlightWord(wIndex);
            }
        }
        #endregion

        #region Loopmode
        public void ReinitLoopmodeInfo()
        {
            Streamer.StartVerse = Displayer.StartVerse;
            Streamer.EndVerse = Displayer.EndVerse;
        }

        public void ReinitLoopmode(bool activated)
        {
            Displayer.SetLoopMode(activated);
            if (activated)
            {
                ReinitLoopmodeInfo();
            }
            Streamer.LoopMode = activated;
        }
        #endregion

        #region Controller Controls
        private void LoopBTN_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_loopMode)
            {
                LoopBtnPath.Fill = Brushes.DarkGray;
                LoopBtnPath.Stroke = Brushes.DarkGray;
            }
            else
            {
                LoopBtnPath.Fill = Brushes.Goldenrod;
                LoopBtnPath.Stroke = Brushes.Goldenrod;
            }

            _loopMode = !_loopMode;

            ReinitLoopmode(_loopMode);
        }

        private void CheikhTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_cheikhModification)
            {
                CheikhTB.Foreground = Brushes.Black;
                ClosePlayer();
            }
            else
            {
                using (new Utils.WaitCursor())
                {
                    if (_surahModification)
                    {
                        SurahTB.Foreground = Brushes.Black;
                        _surahModification = false;
                        SwitchTab(0);
                    }
                    else
                    {
                        OpenPlayer(0);
                    }
                }

                CheikhTB.Foreground = Brushes.Gray;
            }

            _cheikhModification = !_cheikhModification;
        }

        private void SurahTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_surahModification)
            {
                SurahTB.Foreground = Brushes.Black;
                ClosePlayer();
            }
            else
            {
                using (new Utils.WaitCursor())
                {
                    if (_cheikhModification)
                    {
                        CheikhTB.Foreground = Brushes.Black;
                        _cheikhModification = false;
                        SwitchTab(1);
                    }
                    else
                    {
                        OpenPlayer(1);
                    }
                }

                SurahTB.Foreground = Brushes.Gray;
            }

            _surahModification = !_surahModification;
        }

        private void PlayPauseBTN_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Streamer.Verse == 0 || Streamer.Verse == 1) // TODO
            {
                Displayer.ScrollToTop();
            }

            Playing = !_playing;
        }

        private void RefreshPlayPauseBtn()
        {
            if (!_playing)
            {
                PlayPauseBtnPath.Style = (Style)this.Resources["Play"];
            }
            else
            {
                PlayPauseBtnPath.Style = (Style)this.Resources["Pause"];
            }
        }

        private async void BackwardBTN_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Playing = false;
            await ChangeSelectedSurahAsync(LoadedData.SurahList.ElementAt(_selectedSurah.SurahNumber - 1 - 1).Key);
        }

        private async void ForwardBTN_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Playing = false;
            await ChangeSelectedSurahAsync(LoadedData.SurahList.ElementAt(_selectedSurah.SurahNumber + 1 - 1).Key);
        }
        #endregion

        #region Scroll
        private void MainSB_OnScroll(object sender, EventArgs e)
        {
            if (_lastTabShown == 0) // Cheikh selector
            {
                _cheikhSelector.PageSV.ScrollToVerticalOffset(_cheikhSelector.PageSV.ScrollableHeight * MainSB.Scrolled);
            }
            else if (_lastTabShown == 1) // Surah selector
            {
                _surahSelector.PageSV.ScrollToVerticalOffset(_surahSelector.PageSV.ScrollableHeight * MainSB.Scrolled);
            }
        }

        internal void PageSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var sv = (ScrollViewer)sender;
            MainSB.Scrolled = sv.VerticalOffset / sv.ScrollableHeight;
        }
        
        #endregion

        #region Scrollviewer Display
        // `tab` parameter: 0 => Cheikh selector
        //                  1 => Surah selector
        private void OpenPlayer(int tab)
        {
            if (_closing) return;

            _wasPlaying = _playing;

            if (_playing)
            {
                Playing = false;
                RefreshPlayPauseBtn();
            }

            SelectorGrid.Visibility = Visibility.Visible;
            SelectorGrid.Opacity = 0;
            ((Storyboard)this.Resources["PlayerOpenStory"]).Begin();

            SwitchTab(tab, false);

            PlayPauseBTN.IsEnabled = false;
            PlayPauseBTN.Opacity = 0.4;
        }

        private void ClosePlayer()
        {
            _closing = true;
            ((Storyboard)this.Resources["PlayerCloseStory"]).Begin();

            PlayPauseBTN.IsEnabled = true;
            PlayPauseBTN.Opacity = 1;

            if (IsEnabled)
            {
                if (_wasPlaying)
                {
                    Playing = true;
                }
                else
                {
                    Playing = false;
                }
            }
        }

        private void SwitchTab(int tab, bool animation = true)
        {
            if (animation)
            {
                ((Storyboard)this.Resources["TabSwitchStory"]).Begin();
            }

            if (_lastTabShown == tab)
            {
                if (tab == 1)
                {
                    // Refresh surah selector anyways
                    _surahSelector.RefreshSelection();
                }

                return;
            }
            else
            {
                MainSB.ResetThumbY();
            }

            switch (tab)
            {
                case 0:
                    ShowCheikhSelector();
                    break;
                case 1:
                    ShowSurahSelector();
                    break;
                default:
                    throw new NotImplementedException();
            }

            _lastTabShown = tab;
        }

        #region Cheikh Selector
        private void ShowCheikhSelector()
        {
            MainSB.TargetValue = (int)Math.Ceiling(LoadedData.CheikhList.Length / 3d);
            MainSB.Accuracy = ScrollAccuracyMode.ACCURATE;

            if (!_cheikhSelector.ItemsInitialized)
                _cheikhSelector.InitializeItems(this);

            FrameComponent.Content = _cheikhSelector;
        }

        public void ChangeSelectedCheikh(CheikhDescription cheikh)
        {
            _selectedCheikh = cheikh;
            Streamer.Cheikh = cheikh;
            CheikhTB.Text = cheikh.ToString();
        }

        public void ChangeSelectedCheikhCard(CheikhCard card)
        {
            if (card != _selectedCheikhCard)
            {
                if (_selectedCheikhCard != null)
                    _selectedCheikhCard.Unselect();

                ChangeSelectedCheikh(card.Description);

                _selectedCheikhCard = card;
            }
        }
        #endregion

        #region Surah Selector
        public async Task SetSelectedBarAsync(SurahBar bar)
        {
            if (bar != _selectedSurahBar)
            {
                await ChangeSelectedSurahAsync(bar.Surah, true, false);
                if (_selectedSurahBar != null)
                    _selectedSurahBar.Unselect();
                _selectedSurahBar = bar;
            }
        }
        public async Task ChangeSelectedSurahAsync(SurahDescription description, bool loadInDisplayer = true, bool refreshSelectorScroll = true, bool init = false, bool loadLastBookmark = true)
        {
            if (description != _selectedSurah)
            {
                _selectedSurah = description;

                if (description != Streamer.Surah)
                {
                    Streamer.Surah = description;
                    Streamer.Reset();
                }

                // Change backward/forward buttons opacities
                if (description.SurahNumber == 1)
                {
                    BackwardBTN.IsEnabled = false;
                    BackwardBTN.Opacity = 0.4;
                }
                else if (description.SurahNumber == 114)
                {
                    ForwardBTN.IsEnabled = false;
                    ForwardBTN.Opacity = 0.4;
                }
                else
                {
                    BackwardBTN.IsEnabled = true;
                    ForwardBTN.IsEnabled = true;
                    BackwardBTN.Opacity = 1;
                    ForwardBTN.Opacity = 1;
                }

                SurahTB.Text = _selectedSurah.PhoneticName;

                if (loadInDisplayer && !init)
                {
                    await Displayer.LoadSurahAsync(_selectedSurah, false, loadLastBookmark);
                }

                _surahSelector.RefreshSelection(refreshSelectorScroll);

                SurahTB.ToolTip = Utils.General.GenerateSynopsis(_selectedSurah);
            }
        }

        private void ShowSurahSelector()
        {
            MainSB.TargetValue = (int)Math.Ceiling(114 / (LoadedData.CheikhList.Length / 3d));
            MainSB.Accuracy = ScrollAccuracyMode.VAGUE;

            if (!_surahSelector.ItemsInitialized)
            {
                _surahSelector.InitializeItems(this);
            }
            else
            {
                _surahSelector.RefreshSelection();
            }

            FrameComponent.Content = _surahSelector;
        }
        #endregion

        #endregion

        #region UI Reactivity
        private void userControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsEnabled)
            {
                // Close player
                if (_surahModification) SurahTB_PreviewMouseLeftButtonUp(null, null);
                if (_cheikhModification) CheikhTB_PreviewMouseLeftButtonUp(null, null);
            }
        }
        #endregion

        #region Download
        public void DownloadOneVerse(int verseNum)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Fichier MP3|*.mp3";

            if (_selectedSurah.HasBasmala())
            {
                sfd.Title = $"Enregistrer le verset [{verseNum}] de cette sourate";
                sfd.FileName = $"{_selectedCheikh.LastName.Replace(" ", "").ToLower()}_{_selectedSurah.SurahNumber}_{verseNum}";
            }
            else
            {
                sfd.Title = $"Enregistrer le verset [{verseNum+1}] de cette sourate";
                sfd.FileName = $"{_selectedCheikh.LastName.Replace(" ", "").ToLower()}_{_selectedSurah.SurahNumber}_{verseNum+1}";
            }

            if (sfd.ShowDialog() == true)
            {
                string url = StreamingUtils.GenerateVerseUrl(_selectedCheikh, _selectedSurah, verseNum);

                using (var wc = new WebClient())
                {
                    try
                    {
                        using (new Utils.WaitCursor())
                            wc.DownloadFile(url, sfd.FileName);
                    }
                    catch (WebException ex)
                    {
                        Utils.Emergency.ShowExMessage(ex);
                        return;
                    }
                }

                MessageBox.Show(
                    $"Le verset a été téléchargé avec succès\n{sfd.FileName}",
                    "Baraka",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        public void DownloadManyVerses(int begin, int end)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Fichier MP3|*.mp3";

            if (!_selectedSurah.HasBasmala())
            {
                begin++;
                end++;
            }

            sfd.Title = $"Enregistrer en un seul fichier les versets [{begin} à {end}] de cette sourate";
            sfd.FileName = $"{_selectedCheikh.LastName.Replace(" ", "").ToLower()}_{_selectedSurah.SurahNumber}_{begin}-{end}";

            if (sfd.ShowDialog() == true)
            {
                var parts = new List<byte[]>();
                    
                using (var wc = new WebClient())
                {
                    for (int i = begin; i < end+1; i++)
                    {
                        string url = StreamingUtils.GenerateVerseUrl(_selectedCheikh, _selectedSurah, i);
                        try
                        {
                            using (new Utils.WaitCursor())
                                parts.Add(wc.DownloadData(url));
                        }
                        catch (WebException ex)
                        {
                            Utils.Emergency.ShowExMessage(ex);
                            return;
                        }
                    }
                }

                Utils.Audio.Combine(parts, sfd.FileName);
                MessageBox.Show(
                    $"Les versets ont été téléchargés avec succès\n{sfd.FileName}",
                    "Baraka",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }
        #endregion
    }
}