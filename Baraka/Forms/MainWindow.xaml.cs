using Baraka.Data.Descriptions;
using Baraka.Forms;
using Baraka.Forms.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

// todo remove

namespace Baraka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _mainGridScale = 1;

        public MainWindow()
        {
            InitializeComponent();

            ((Storyboard)this.Resources["DashboardCloseStory"]).Begin();
            ((Storyboard)this.Resources["DashboardCloseStory"]).SkipToFill();

            // Bind the displayer to the player
            Player.Displayer = MainSurahDisplayer;
        }

        private Dictionary<SurahDescription, Data.Surah.SurahVersion[]> SerializationData =
            new Dictionary<SurahDescription, Data.Surah.SurahVersion[]>();

        private void Window_Initialized(object sender, EventArgs e)
        {
            /*string translationPath = @"C:\Users\jomtek360\Documents\Baraka\quran-translated-main";
            foreach (var elem in LoadedData.SurahList)
            {
                var desc = elem.Key;
                var surahNum = desc.SurahNumber;
                var arVersion = File.ReadAllLines($@"{translationPath}\arabic_uthmani\{surahNum}");
                var phVersion = File.ReadAllLines($@"{translationPath}\phonetic\{surahNum}");
                var frVersion = File.ReadAllLines($@"{translationPath}\french_hamidullah\{surahNum}");

                SerializationData.Add(desc, new Data.Surah.SurahVersion[]
                {
                    new Data.Surah.SurahVersion("arabic", "uthmani", arVersion),
                    new Data.Surah.SurahVersion("phonetic", "", phVersion),
                    new Data.Surah.SurahVersion("french", "hamidullah", frVersion),
                });
            }
            Data.SerializationUtils.Serialize(SerializationData, "qurannew.ser");*/
            
            
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
                new CheikhDescription("Mahmoud Khalil", "Al-Hussary", "https://everyayah.com/data/Husary_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/hussary.png"))),
                new CheikhDescription("Mahmoud Khalil", "Al-Hussary مجود", "https://everyayah.com/data/Husary_128kbps_Mujawwad", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/hussary.png"))),
                new CheikhDescription("Abdullah", "Basfar", "https://everyayah.com/data/Abdullah_Basfar_192kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/basfar.png"))),
                new CheikhDescription("Salah", "Al-Budair", "https://everyayah.com/data/Salah_Al_Budair_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/budair.png"))),
                new CheikhDescription("Hani", "Rifai", "https://everyayah.com/data/Hani_Rifai_192kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/rifai.png"))),
                new CheikhDescription("Abdallah", "Matroud", "https://everyayah.com/data/Abdullah_Matroud_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/matroud.png"))),
                new CheikhDescription("Ayman", "Suwaid", "https://everyayah.com/data/Ayman_Sowaid_64kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/suwaid.png"))),
                new CheikhDescription("Sahl", "Yassin", "https://everyayah.com/data/Sahl_Yassin_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/yassin.png"))),
            };

            Data.SerializationUtils.Serialize(tempCheikhs, "cheikh_new.ser");
        
        }

        #region Window events
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Player.Dispose();
            Environment.Exit(0);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetMinWidth();
            ApplyScale(true);
        }
        #endregion

        #region Displayer to Player events
        private void MainSurahDisplayer_VerseChanged(object sender, int num)
        {
            // TODO : is this function used
            Player.ChangeVerse(num);
        }

        private void MainSurahDisplayer_DownloadVerseRequested(object sender, int num)
        {

            Player.DownloadMp3Verse(num);
        }
        #endregion

        #region Dashboard
        private bool _dashboardOpened = false;

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

            var myInstance = new SearchWindow() { Owner = this };
            myInstance.VerseClicked += (object _, VerseDescription verse) =>
            {
                Player.ChangeSelectedSurah(verse.Surah);
                Player.ChangeVerse(verse.Number);

                MainSurahDisplayer.BrowseToVerse(verse.Number);
                MainSurahDisplayer.ScrollToVerse(verse.Number, true);
                
                Player.Playing = false;
            };
            myInstance.ShowDialog();

            WindowBlurEffect.Radius = 0;
        }

        private void SettingsDashboardItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowBlurEffect.Radius = 8;
            new SettingsWindow() { Owner = this }.ShowDialog();
            WindowBlurEffect.Radius = 0;
        }
        #endregion
        #endregion

        #region Zoom
        private double GetDisplayerWidth()
        {
            return MainSurahDisplayer.ActualWidth * ScaleTransformer.ScaleX;
        }
        private void SetMinWidth() // Set minimum width
        {
            MinWidth = GetDisplayerWidth() + Dashboard.ActualWidth + 50;
        }

        private void EditScale(double change)
        {
            var final = _mainGridScale + change;

            if (final > 0.8 && final < 1.5)
            {
                _mainGridScale += change;
            }
        }

        // TODO
        private void ApplyScale(bool recursive = false)
        {
            if (GetDisplayerWidth() + 50 + Dashboard.ActualWidth < ActualWidth)
            {
                ScaleTransformer.ScaleX = _mainGridScale;
                ScaleTransformer.ScaleY = _mainGridScale;
            }
            else
            {
                EditScale(-0.025);
                ScaleTransformer.ScaleX = _mainGridScale;
                ScaleTransformer.ScaleY = _mainGridScale;
                if (recursive) ApplyScale(true);
            }
        }

        private void MainGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta > 0)
            {
                EditScale(0.025);
            }
            else if (e.Delta < 0)
            {
                EditScale(-0.025);
            }

            ApplyScale();
            SetMinWidth();
        }
        #endregion
    }
}