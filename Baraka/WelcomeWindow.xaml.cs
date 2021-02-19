using Baraka.Data;
using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            await Task.Delay(2000);

            TitleTB.Margin = new Thickness(0, 43, 0, 0);
            MainPB.Visibility = Visibility.Visible;
            ProgressionTB.Visibility = Visibility.Visible;

            // // Loading

            // Deserialize data
            LoadedData.SurahList =
                SerializationUtils.Deserialize<Dictionary<SurahDescription, Data.Surah.SurahVersion[]>>("quran.ser");
            MainPB.Progress = 100;

            //  // Finish
            Hide();
            var window = new MainWindow();
            window.MainSurahDisplayer.LoadSurah(LoadedData.SurahList.ElementAt(0).Key);
            window.Show();
        }
    }
}
