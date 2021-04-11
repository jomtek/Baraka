using Baraka.Forms.Settings;
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
using System.Windows.Shapes;

namespace Baraka.Forms.Settings
{
    /// <summary>
    /// Logique d'interaction pour SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private int _selectedItem = 0;
        public SettingsWindow()
        {
            InitializeComponent();
            FrameComponent.Content = new GeneralPage();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Dashboard
        private void SetSelectedItem(int selectedItem)
        {
            General_MenuItem_Grid.Background = Brushes.White;
            Quran_MenuItem_Grid.Background = Brushes.White;
            Appearance_MenuItem_Grid.Background = Brushes.White;

            switch (selectedItem)
            {
                case 0:
                    General_MenuItem_Grid.Background = (SolidColorBrush)FindResource("DashboardSelectedItemBrush");
                    FrameComponent.Content = new GeneralPage();
                    break;
                case 1:
                    Quran_MenuItem_Grid.Background = (SolidColorBrush)FindResource("DashboardSelectedItemBrush");
                    FrameComponent.Content = new QuranPage();
                    break;
                case 2:
                    Appearance_MenuItem_Grid.Background = (SolidColorBrush)FindResource("DashboardSelectedItemBrush");
                    FrameComponent.Content = new AppearancePage();
                    break;
            }

            _selectedItem = selectedItem;
        }

        private void General_MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 0)
                General_MenuItem_Grid.Background = (SolidColorBrush)FindResource("DashboardHoveredItemBrush");
        }

        private void General_MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 0)
                General_MenuItem_Grid.Background = Brushes.White;
        }

        private void Quran_MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 1)
                Quran_MenuItem_Grid.Background = (SolidColorBrush)FindResource("DashboardHoveredItemBrush");
        }

        private void Quran_MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 1)
                Quran_MenuItem_Grid.Background = Brushes.White;
        }

        private void Appearance_MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 2)
                Appearance_MenuItem_Grid.Background = (SolidColorBrush)FindResource("DashboardHoveredItemBrush");
        }

        private void Appearance_MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 2)
                Appearance_MenuItem_Grid.Background = Brushes.White;
        }
        #endregion

        private void General_MenuItem_Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedItem(0);
        }

        private void Quran_MenuItem_Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedItem(1);
        }

        private void Appearance_MenuItem_Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedItem(2);
        }
    }
}
