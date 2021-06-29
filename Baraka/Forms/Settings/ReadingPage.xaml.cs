using Baraka.Data;
using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Baraka.Forms.Settings
{
    /// <summary>
    /// Logique d'interaction pour ReadingPage.xaml
    /// </summary>
    public partial class ReadingPage : Page
    {
        public ReadingPage()
        {
            InitializeComponent();

            LoadSettings();

            // Fill Comboboxes
            foreach (SurahDescription surah in Data.LoadedData.SurahList.Keys)
            {
                DefaultSurahCMBB.Items.Add($"{surah.SurahNumber}. {surah.PhoneticName}");
            }

            foreach (CheikhDescription cheikh in Data.LoadedData.CheikhList)
            {
                DefaultCheikhCMBB.Items.Add(cheikh.ToString());
            }
        }

        #region Load and save
        private void LoadSettings()
        {
            DefaultSurahCMBB.SelectedIndex = LoadedData.Settings.DefaultSurahIndex;
            DefaultCheikhCMBB.SelectedIndex = LoadedData.Settings.DefaultCheikhIndex;
            AutoScrollCHB.IsChecked = LoadedData.Settings.AutoScrollQuran;
            AutoNextCHB.IsChecked = LoadedData.Settings.AutoNextSurah;
            AutoReloadCHB.IsChecked = LoadedData.Settings.AutoReloadLastSurah;
            // Editions...
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            LoadedData.Settings.DefaultSurahIndex = DefaultSurahCMBB.SelectedIndex;
            LoadedData.Settings.DefaultCheikhIndex = DefaultCheikhCMBB.SelectedIndex;
            LoadedData.Settings.AutoScrollQuran = AutoScrollCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.AutoNextSurah = AutoNextCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.AutoReloadLastSurah = AutoReloadCHB.IsChecked.GetValueOrDefault();
        }
        #endregion

    }
}
