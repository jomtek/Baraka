using Baraka.Data;
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
    /// Logique d'interaction pour GeneralPage.xaml
    /// </summary>
    public partial class GeneralPage : Page, ISettingsPage
    {
        public GeneralPage()
        {
            InitializeComponent();

            LaunchOnStartupCHB.IsChecked = LoadedData.Settings.Startup;
            LanguageCMBB.SelectedIndex = (int)LoadedData.Settings.Language;
            AllowCacheCHB.IsChecked = LoadedData.Settings.EnableAudioCache;
            ClearCacheCHB.IsChecked = LoadedData.Settings.ClearAudioCache;
        }

        public void SaveSettings()
        {
            LoadedData.Settings.Startup = LaunchOnStartupCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.Language = (Globalization.Language)LanguageCMBB.SelectedIndex;
            LoadedData.Settings.EnableAudioCache = AllowCacheCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.ClearAudioCache = ClearCacheCHB.IsChecked.GetValueOrDefault();
        }
    }
}
