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
                SerializationUtils.Deserialize<Dictionary<SurahDescription, Data.Surah.SurahVersion[]>>("quran.ser");
            LoadedData.CheikhList =
                SerializationUtils.Deserialize<CheikhDescription[]>("cheikh.ser");
            LoadedData.Settings = new MySettings("settings.ser");

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
