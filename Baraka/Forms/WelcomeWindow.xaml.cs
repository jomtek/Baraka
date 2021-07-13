using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Baraka
{
    /// <summary>
    /// Logique d'interaction pour StartupForm.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        private Dictionary<SurahDescription, Dictionary<string, SurahVersion>> SerializationData =
            new Dictionary<SurahDescription, Dictionary<string, SurahVersion>>();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadedData.Settings =
                SerializationUtils.Deserialize<MySettings>("settings.ser");

            if (!LoadedData.Settings.ShowWelcomeWindow)
            {
                Visibility = Visibility.Hidden;
            }

            MainPB.Visibility = Visibility.Collapsed;
            ProgressionTB.Visibility = Visibility.Collapsed;

            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(800);

            TitleTB.Margin = new Thickness(
                TitleTB.Margin.Left,
                TitleTB.Margin.Top - 40,
                TitleTB.Margin.Right,
                TitleTB.Margin.Bottom
            );

            MainPB.Visibility = Visibility.Visible;
            ProgressionTB.Visibility = Visibility.Visible;

            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(10);

            // // Loading
            //
            // Deserialize data
            ProgressionTB.Text = "chargement des ressources: quran.ser";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(50);
            LoadedData.SurahList =
                SerializationUtils.Deserialize<Dictionary<SurahDescription, Dictionary<string, SurahVersion>>>("data/quran.ser");
            MainPB.Progress = 0.2;

            ProgressionTB.Text = "chargement des ressources: cheikh.ser";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(50);
            LoadedData.CheikhList =
                SerializationUtils.Deserialize<CheikhDescription[]>("data/cheikh.ser");
            MainPB.Progress = 0.4;

            ProgressionTB.Text = "chargement des ressources: translations.ser";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(50);
            LoadedData.TranslationsList =
                SerializationUtils.Deserialize<TranslationDescription[]>("data/translations.ser");
            MainPB.Progress = 0.5;

            ProgressionTB.Text = "chargement des ressources: cache.ser";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(70);
            var cacheContent =
                SerializationUtils.Deserialize<Dictionary<string, byte[]>>("data/cache.ser");
            LoadedData.AudioCache = new AudioCacheManager(cacheContent);
            MainPB.Progress = 0.7;

            // Deserialize settings
            ProgressionTB.Text = "chargement des préférences...";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(350);
            LoadedData.Bookmarks =
                SerializationUtils.Deserialize<List<int>>("bookmarks.ser");
            MainPB.Progress = 1;

            // DEBUG
            /*
            string translationPath = @"C:\Users\jomtek360\Documents\Baraka\quran-translated-main";
            foreach (var elem in SerializationUtils.Deserialize<Dictionary<SurahDescription, List<SurahVersion>>>("data/quran.ser"))
            {
                var desc = elem.Key;
                var surahNum = desc.SurahNumber;
                var arVersion = File.ReadAllLines($@"{translationPath}\arabic_uthmani\{surahNum}");
                var phVersion = File.ReadAllLines($@"{translationPath}\phonetic\{surahNum}");

                SerializationData.Add(desc, new Dictionary<string, SurahVersion>()
                {
                    ["ARABIC"]   = new SurahVersion("ARABIC", arVersion),
                    ["PHONETIC"] = new SurahVersion("PHONETIC", phVersion)
                });
            }
            Data.SerializationUtils.Serialize(SerializationData, "qurannew.ser");

            MessageBox.Show("yes");
            return;
            */


            //  // Finish
            Hide();

            var window = new MainWindow();
            window.Loaded += (object self, RoutedEventArgs a) =>
            {
                var defaultCheikh = LoadedData.CheikhList.ElementAt(LoadedData.Settings.DefaultCheikhIndex);
                var defaultSurah = LoadedData.SurahList.ElementAt(LoadedData.Settings.DefaultSurahIndex).Key;

                window.Player.ChangeSelectedCheikh(defaultCheikh);
                window.Player.ChangeSelectedSurah(defaultSurah);
                
                window.MainSurahDisplayer.LoadSurah(window.Player.SelectedSurah);

                // Other UI settings
                window.MainSurahDisplayer.SetSBVisible(LoadedData.Settings.DisplayScrollBar);
            };

            // TODO: wait for window to be fully loaded before showing it
            window.Show();
        }
    }
}
