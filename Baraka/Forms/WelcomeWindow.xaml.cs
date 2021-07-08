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
            await Task.Delay(700);

            TitleTB.Margin = new Thickness(0, 43, 0, 0);
            MainPB.Visibility = Visibility.Visible;
            ProgressionTB.Visibility = Visibility.Visible;

            await Task.Delay(10);

            // // Loading

            //
            ProgressionTB.Text = "chargement des ressources...";

            await Task.Delay(50);

            // Deserialize data
            LoadedData.SurahList =
                SerializationUtils.Deserialize<Dictionary<SurahDescription, Dictionary<string, SurahVersion>>>("data/quran.ser");
            ProgressionTB.Text = "chargement des ressources: quran.ser";
            await Task.Delay(50);
            MainPB.Progress = 0.2;
            
            LoadedData.CheikhList =
                SerializationUtils.Deserialize<CheikhDescription[]>("data/cheikh.ser");
            ProgressionTB.Text = "chargement des ressources: cheikh.ser";
            await Task.Delay(50);
            MainPB.Progress = 0.4;
            
            LoadedData.TranslationsList =
                SerializationUtils.Deserialize<TranslationDescription[]>("data/translations.ser");
            ProgressionTB.Text = "chargement des ressources: translations.ser";
            await Task.Delay(50);


            //
            ProgressionTB.Text = "chargement des préférences...";
            MainPB.Progress = 0.5;
            await Task.Delay(30);
            
            // Init cache container
            LoadedData.AudioCache = new Dictionary<string, byte[]>();

            // Deserialize settings
            LoadedData.Settings =
                new MySettings("data/settings.ser");
            LoadedData.Bookmarks =
                SerializationUtils.Deserialize<List<int>>("bookmarks.ser");

            // Debug
            MainPB.Progress = 1;
            
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
