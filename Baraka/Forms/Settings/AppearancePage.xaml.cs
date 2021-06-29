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
    /// Logique d'interaction pour AppearancePage.xaml
    /// </summary>
    public partial class AppearancePage : Page
    {
        public AppearancePage()
        {
            InitializeComponent();
            LoadSettings();
        }

        #region Load and save
        private void LoadSettings()
        {
            ShowWelcomeWindowCHB.IsChecked = LoadedData.Settings.ShowWelcomeWindow;
            DisplayScrollbarCHB.IsChecked = LoadedData.Settings.DisplayScrollBar;
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            LoadedData.Settings.ShowWelcomeWindow = ShowWelcomeWindowCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.DisplayScrollBar = DisplayScrollbarCHB.IsChecked.GetValueOrDefault();
        }
        #endregion
    }
}
