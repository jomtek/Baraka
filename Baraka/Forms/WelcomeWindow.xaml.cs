using Baraka.Data;
using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
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
                SerializationUtils.Deserialize<Dictionary<SurahDescription, Data.Surah.SurahVersion[]>>("data/quran.ser");
            LoadedData.CheikhList =
                SerializationUtils.Deserialize<CheikhDescription[]>("data/cheikh.ser");
            LoadedData.Settings =
                new MySettings("data/settings.ser");
            LoadedData.Bookmarks =
                SerializationUtils.Deserialize<List<int>>("data/bookmarks.ser");

            MainPB.Progress = 100;
            

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
