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
    /// Logique d'interaction pour SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();

            LoadSettings();

            // Fill Comboboxes
            // Pick all editions available for Al-Fatiha
            foreach (var edition in LoadedData.SurahList.ElementAt(0).Value.Keys)
            {
                SearchEditionCMBB.Items.Add(edition);
                ResultsEditionCMBB.Items.Add(edition);
            }
        }

        #region Load and save
        private void LoadSettings()
        {
            SearchEditionCMBB.SelectedItem = LoadedData.Settings.SearchEdition;
            ResultsEditionCMBB.SelectedItem = LoadedData.Settings.ResultsEdition;
            HighlightKeywordsCHB.IsChecked = LoadedData.Settings.HighlightSearchKeywords;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            LoadedData.Settings.SearchEdition = SearchEditionCMBB.Text;
            LoadedData.Settings.ResultsEdition = ResultsEditionCMBB.Text;
            LoadedData.Settings.HighlightSearchKeywords = HighlightKeywordsCHB.IsChecked.GetValueOrDefault();
        }
        #endregion

    }
}
