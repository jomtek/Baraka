using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using Baraka.Forms;
using Baraka.Forms.Settings;
using Baraka.Theme.UserControls.Quran.Display;
using Baraka.Theme.UserControls.Quran.Display.Translated;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Baraka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _zoomScale = 1;

        #region Events
        [Category("Baraka")]
        public event EventHandler<double> LoadingProgressChanged;
        #endregion

        public void ReportLoadingProgress(double progress)
        {
            LoadingProgressChanged?.Invoke(this, progress);
        }

        public MainWindow()
        {
            InitializeComponent();

            ((Storyboard)this.Resources["DashboardCloseStory"]).Begin();
            ((Storyboard)this.Resources["DashboardCloseStory"]).SkipToFill();

            // Bind the displayer to the player
            Player.Displayer = TranslatedSurahDisplayer;
            Player.Displayer.EnabledChanged += (object sender, EventArgs e) =>
            {
                if (!TranslatedSurahDisplayer.IsEnabled)
                {
                    Player.Playing = false;
                    TranslatedSurahDisplayer.Playing = false;
                }
                Dashboard.IsEnabled = TranslatedSurahDisplayer.IsEnabled;
                Player.IsEnabled = TranslatedSurahDisplayer.IsEnabled;
            };
        }

        #region Serialize (temp)
        private Dictionary<SurahDescription, List<SurahVersion>> SerializationData =
            new Dictionary<SurahDescription, List<SurahVersion>>();

        private void Window_Initialized(object sender, EventArgs e)
        {
            return;
            var tempCheikhs = new CheikhDescription[]
            {
                new CheikhDescription("Mishary bin Rashid", "Alafasy", "https://everyayah.com/data/Alafasy_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/alafasy.png"))),
                new CheikhDescription("Abderrahman", "Al-Sudais", "https://everyayah.com/data/Abdurrahmaan_As-Sudais_192kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/soudais.png"))),
                new CheikhDescription("Maher", "Al-Mueaqly", "https://everyayah.com/data/MaherAlMuaiqly128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/maher.png"))),
                new CheikhDescription("Saad", "Al-Ghamidi", "https://everyayah.com/data/Ghamadi_40kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/ghamidi.png"))),
                new CheikhDescription("Saleh", "Bukhatir", "https://everyayah.com/data/Salaah_AbdulRahman_Bukhatir_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/bukhatir.png"))),
                new CheikhDescription("Yasser", "Al-Dosari", "https://everyayah.com/data/Yasser_Ad-Dussary_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/dosari.png"))),
                new CheikhDescription("Muhammad Siddiq", "Minshawi مجود", "https://everyayah.com/data/Minshawy_Mujawwad_192kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/minshawi.png"))),
                new CheikhDescription("Muhammad Siddiq", "Minshawi مرتل", "https://everyayah.com/data/Minshawy_Murattal_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/minshawi.png"))),
                new CheikhDescription("Abdul Basit", "Abdessamad مجود", "https://everyayah.com/data/Abdul_Basit_Mujawwad_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/abdessamad.png"))),
                new CheikhDescription("Abdul Basit", "Abdessamad مرتل", "https://everyayah.com/data/Abdul_Basit_Murattal_192kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/abdessamad.png"))),
                new CheikhDescription("Saoud", "Shuraim", "https://everyayah.com/data/Saood_ash-Shuraym_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/shuraim.png"))),
                new CheikhDescription("Fares", "Abbad", "https://everyayah.com/data/Fares_Abbad_64kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/abbad.png"))),
                new CheikhDescription("Ali", "Jaber", "https://everyayah.com/data/Ali_Jaber_64kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/jaber.png"))),
                new CheikhDescription("Abdullah", "Al-Juhany", "https://everyayah.com/data/Abdullaah_3awwaad_Al-Juhaynee_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/juhany.png"))),
                new CheikhDescription("Mahmoud Khalil", "Al-Hussary", "https://everyayah.com/data/Husary_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/hussary.png"))),
                new CheikhDescription("Mahmoud Khalil", "Al-Hussary مجود", "https://everyayah.com/data/Husary_128kbps_Mujawwad", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/hussary.png"))),
                new CheikhDescription("Abdullah", "Basfar", "https://everyayah.com/data/Abdullah_Basfar_192kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/basfar.png"))),
                new CheikhDescription("Salah", "Al-Budair", "https://everyayah.com/data/Salah_Al_Budair_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/budair.png"))),
                new CheikhDescription("Hani", "Rifai", "https://everyayah.com/data/Hani_Rifai_192kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/rifai.png"))),
                new CheikhDescription("Abdallah", "Matroud", "https://everyayah.com/data/Abdullah_Matroud_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/matroud.png"))),
                new CheikhDescription("Ali", "Hudhaify", "https://everyayah.com/data/Hudhaify_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/hudhaify.png"))),
                new CheikhDescription("Sahl", "Yassin", "https://everyayah.com/data/Sahl_Yassin_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/yassin.png"))),
            };

            SerializationUtils.Serialize(tempCheikhs, "cheikh_new.ser");

            /*
            var tempTranslations = new List<TranslationDescription>();

            foreach (var line in System.IO.File.ReadAllLines("translations.txt"))
            {
                string[] info = line.Split('_');
                var translation = new TranslationDescription(info[0], info[1], info[2], info[3], info[4], new BitmapImage(new Uri($@"pack://application:,,,/Baraka;component/Images/Flags/{info[0]}.png")));
                tempTranslations.Add(translation);
            }

            SerializationUtils.Serialize(tempTranslations.ToArray(), "translations_new.ser");
            */

        }
        #endregion

        #region Handlers
        #region Window events
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Player.Dispose();

            // Serialize stuff that should be serialized
            //
            if (LoadedData.Settings.ClearAudioCache)
            {
                LoadedData.AudioCache.Clear();
            }

            if (LoadedData.Settings.AutoReloadLastSurah)
            {
                LoadedData.Settings.DefaultCheikhIndex = Array.IndexOf(LoadedData.CheikhList, Player.SelectedCheikh);
                LoadedData.Settings.DefaultSurahIndex = Player.SelectedSurah.SurahNumber - 1;
            }

            SerializationUtils.Serialize(LoadedData.Settings, "settings.ser");
            SerializationUtils.Serialize(LoadedData.Bookmarks, "bookmarks.ser");
            SerializationUtils.Serialize(LoadedData.AudioCache.Content, "data/cache.ser");

            // DEBUG
            //SerializationUtils.Serialize(LoadedData.MushafGlyphProvider.GlyphInfoDict, "glyph_info.ser");

            // Exit
            Environment.Exit(0);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetMinWidth();
            ApplyScale(true);
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Prevent tab navigation
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
            }
            // Manage book navigation
            else if ((e.Key == Key.Left || e.Key == Key.Right) && !Player.IsOpened)
            {
                await Task.Delay(150);

                if (Keyboard.IsKeyDown(e.Key))
                {
                    _intensity *= 1.01;
                    await MushafSurahDisplayer.NaturalBrowse(CheckIfNext(e.Key), Convert.ToInt32(2 * _intensity));
                }
                else
                {
                    _intensity = 1;
                    await MushafSurahDisplayer.NaturalBrowse(CheckIfNext(e.Key));
                }
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            InitWindows();
        }
        #endregion

        #region Displayer to Player
#pragma warning disable IDE0051 // Remove unused private members
        private void TranslatedSurahDisplayer_VerseChanged(object sender, int num)
        {
            Player.ChangeVerse(num);
        }

        private void TranslatedSurahDisplayer_DownloadRecitationRequested(object sender, DownloadRecitationEventArgs e)
        {
            if (e.Begin == e.End)
            {
                Player.DownloadOneVerse(e.Begin);
            }
            else
            {
                Player.DownloadManyVerses(e.Begin, e.End);
            }
        }
#pragma warning restore IDE0051
        #endregion
        #endregion

        #region Dashboard
        private bool _dashboardOpened = false;
        private SearchWindow _searchWindow;
        private SettingsWindow _settingsWindow;

        private void InitWindows()
        {
            // Search window
            _searchWindow = new SearchWindow() { Owner = this };
            _searchWindow.VerseClicked += (object _, VerseDescription verse) =>
            {
                IntersurahChangeVerseAsync(verse, true).ConfigureAwait(false);
            };

            // Settings window
            _settingsWindow = new SettingsWindow() { Owner = this };
        }

        #region Animation
        private void Dashboard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!_dashboardOpened)
            {
                ((Storyboard)this.Resources["DashboardOpenStory"]).Begin();
                _dashboardOpened = true;
            }
        }

        private void Dashboard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_dashboardOpened)
            {
                ((Storyboard)this.Resources["DashboardCloseStory"]).Begin();
                _dashboardOpened = false;
            }
        }
        #endregion
        #region Items
        private void SearchDashboardItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowBlurEffect.Radius = 8;

            // TODO: apply placeholder directly in SearchWindow.cs
            _searchWindow.SearchTB.Placeholder = $"rechercher ({LoadedData.Settings.SearchEdition})...";
            _searchWindow.ShowDialog();

            WindowBlurEffect.Radius = 0;
        }

        private async void SettingsDashboardItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var oldSurahVerConfig =
                (SurahVersionConfig)LoadedData.Settings.SurahVersionConfig.Clone();

            WindowBlurEffect.Radius = 8;
            _settingsWindow.ShowDialog();
            WindowBlurEffect.Radius = 0;

            // Apply settings to be directly applied
            //
            TranslatedSurahDisplayer.SetSBVisible(LoadedData.Settings.DisplayScrollBar);
            Player.Streamer.OutputDeviceIndex = LoadedData.Settings.OutputDeviceIndex;

            if (!LoadedData.Settings.SurahVersionConfig.Equals(oldSurahVerConfig))
            {
                // Reload actual surah
                await TranslatedSurahDisplayer.LoadSurahAsync(TranslatedSurahDisplayer.Surah, true, false);
            }
        }
        #endregion
        #endregion

        #region Zoom
        private double GetTranslatedDisplayerWidth()
        {
            return TranslatedSurahDisplayer.ActualWidth * ScaleTransformer.ScaleX;
        }
        private void SetMinWidth() // Set minimum width
        {
            MinWidth = GetTranslatedDisplayerWidth() + Dashboard.ActualWidth + 50;
        }

        private void EditScale(double change, bool mushaf)
        {
            var final = _zoomScale + change;

            // Zoom limitations
            if (!mushaf && (final < 1 || final > 1.7))
            {
                return;
            }
            else
            {
                _zoomScale += change;
            }

            if (mushaf && _zoomScale < 1)
            {
                _zoomScale = 1;
            }
        }

        // TODO
        private void ApplyScale(bool mushaf, bool recursive = false)
        {
            if (mushaf) // Mushaf displayer
            {
                Console.WriteLine("yes");
                MushafSurahDisplayer.ApplyScale(_zoomScale);
            }
            else // Translated displayer
            {
                if (GetTranslatedDisplayerWidth() + 50 + Dashboard.ActualWidth < ActualWidth)
                {
                    ScaleTransformer.ScaleX = _zoomScale;
                    ScaleTransformer.ScaleY = _zoomScale;
                }
                else
                {
                    EditScale(-0.025, mushaf);
                    ScaleTransformer.ScaleX = _zoomScale;
                    ScaleTransformer.ScaleY = _zoomScale;
                    if (recursive) ApplyScale(true);
                }
            }
        }

        private void MainGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            bool mushaf = LoadedData.Settings.SurahVersionConfig.ShowMushaf();

            if (e.Delta > 0)
            {
                EditScale(0.025, mushaf);
            }
            else if (e.Delta < 0)
            {
                EditScale(-0.025, mushaf);
            }

            ApplyScale(mushaf);
            SetMinWidth();
        }
        #endregion

        #region Display
        public async Task ChangeDisplayModeAsync(QuranDisplayMode mode)
        {
            if (mode == QuranDisplayMode.MUSHAF)
            {
                /*MessageBox.Show("Nous sommes désolés: le mode Mus'haf n'est pas encore disponible.\n" +
                    "Cependant, nous affichons quand-même la version arabe pour servir vos ~petits yeux~\n\n",
                    "Message du développeur");*/
                //TranslatedSurahDisplayer.UnloadActualSurah();
                await MushafSurahDisplayer.LoadSurahAsync(Player.SelectedSurah);
                
                TranslatedSurahDisplayer.Visibility = Visibility.Collapsed;
                MushafSurahDisplayer.Visibility = Visibility.Visible;
            
            }
            else if (mode == QuranDisplayMode.TRANSLATED)
            {
                MushafSurahDisplayer.UnloadActualSurah();
                await TranslatedSurahDisplayer.LoadSurahAsync(Player.SelectedSurah);
                
                MushafSurahDisplayer.Visibility = Visibility.Collapsed;
                TranslatedSurahDisplayer.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region Local utils
        public async Task IntersurahChangeVerseAsync(VerseDescription verse, bool searchRes = false)
        {
            Player.Playing = false;
            
            await Player.ChangeSelectedSurahAsync(verse.Surah, true, false, false, false);
            Player.ChangeVerse(verse.Number);

            await TranslatedSurahDisplayer.BrowseToVerseAsync(verse.Number);
            await TranslatedSurahDisplayer.ScrollToVerseAsync(verse.Number, searchRes);
        }
        #endregion

        private async void Window_KeyUp(object sender, KeyEventArgs e)
        {
            // TODO: maybe this part is temporary
            
        }

        private bool CheckIfNext(Key key)
        {
            return key == Key.Left;
        }

        private double _intensity = 1;
        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
           
        }
    }
}