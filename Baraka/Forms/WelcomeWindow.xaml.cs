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
            await Task.Delay(1500);

            TitleTB.Margin = new Thickness(0, 43, 0, 0);
            MainPB.Visibility = Visibility.Visible;
            ProgressionTB.Visibility = Visibility.Visible;

            await Task.Delay(10);
            
            // // Loading
            // Deserialize data
            LoadedData.SurahList =
                SerializationUtils.Deserialize<Dictionary<SurahDescription, Dictionary<string, SurahVersion>>>("data/quran.ser");
            LoadedData.CheikhList =
                SerializationUtils.Deserialize<CheikhDescription[]>("data/cheikh.ser");
            LoadedData.TranslationsList =
                SerializationUtils.Deserialize<TranslationDescription[]>("data/translations.ser");

            MainPB.Progress = 50;

            // Deserialize settings
            LoadedData.Settings =
                new MySettings("data/settings.ser");
            LoadedData.Bookmarks =
                SerializationUtils.Deserialize<List<int>>("bookmarks.ser");

            // Debug
            MainPB.Progress = 100;
            
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
            window.Loaded += (object _, RoutedEventArgs a) =>
            {
                window.MainSurahDisplayer.LoadSurah(LoadedData.SurahList.ElementAt(0).Key);
            };

            window.Show();
        }
    }
}
