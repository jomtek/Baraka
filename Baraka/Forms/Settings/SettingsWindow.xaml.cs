using Baraka.Data;
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
        private Page[] _pages;

        public SettingsWindow()
        {
            InitializeComponent();
            
            _pages = new Page[]
            {
                new GeneralPage(),
                new AppearancePage(),
                new ReadingPage(),
                new SearchPage(),
                new HelpPage(),
            };

            SetSelectedTab(LoadedData.Settings.SelectedTab);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            // TODO: remove this event
        }

        #region Dashboard
        private int _selectedItem = 0;

        private void SetSelectedTab(int tab)
        {
            switch (tab)
            {
                case 0:
                    GeneralTB.Foreground = Brushes.Gray;
                    AppearanceTB.Foreground = Brushes.Black;
                    ReadingTB.Foreground = Brushes.Black;
                    ResearchTB.Foreground = Brushes.Black;
                    HelpTB.Foreground = Brushes.Black;
                    break;
                case 1:
                    AppearanceTB.Foreground = Brushes.Gray;
                    GeneralTB.Foreground = Brushes.Black;
                    ReadingTB.Foreground = Brushes.Black;
                    ResearchTB.Foreground = Brushes.Black;
                    HelpTB.Foreground = Brushes.Black;
                    break;
                case 2:
                    ReadingTB.Foreground = Brushes.Gray;
                    AppearanceTB.Foreground = Brushes.Black;
                    GeneralTB.Foreground = Brushes.Black;
                    ResearchTB.Foreground = Brushes.Black;
                    HelpTB.Foreground = Brushes.Black;
                    break;
                case 3:
                    ResearchTB.Foreground = Brushes.Gray;
                    ReadingTB.Foreground = Brushes.Black;
                    AppearanceTB.Foreground = Brushes.Black;
                    GeneralTB.Foreground = Brushes.Black;
                    HelpTB.Foreground = Brushes.Black;
                    break;
                case 4:
                    HelpTB.Foreground = Brushes.Gray;
                    ResearchTB.Foreground = Brushes.Black;
                    ReadingTB.Foreground = Brushes.Black;
                    AppearanceTB.Foreground = Brushes.Black;
                    GeneralTB.Foreground = Brushes.Black;
                    break;
            }

            _selectedItem = tab;
            FrameComponent.Content = _pages[tab];

            LoadedData.Settings.SelectedTab = tab;
        }

        #region Hover Handlers
        private void GeneralTB_MouseEnter(object sender, MouseEventArgs e)
        {
            GeneralTB.Foreground = Brushes.Gray;
        }

        private void GeneralTB_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 0)
            {
                GeneralTB.Foreground = Brushes.Black;
            }
        }

        private void AppearanceTB_MouseEnter(object sender, MouseEventArgs e)
        {
            AppearanceTB.Foreground = Brushes.Gray;
        }

        private void AppearanceTB_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 1)
            {
                AppearanceTB.Foreground = Brushes.Black;
            }
        }

        private void ReadingTB_MouseEnter(object sender, MouseEventArgs e)
        {
            ReadingTB.Foreground = Brushes.Gray;
        }

        private void ReadingTB_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 2)
            {
                ReadingTB.Foreground = Brushes.Black;
            }
        }

        private void ResearchTB_MouseEnter(object sender, MouseEventArgs e)
        {
            ResearchTB.Foreground = Brushes.Gray;
        }

        private void ResearchTB_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 3)
            {
                ResearchTB.Foreground = Brushes.Black;
            }
        }

        private void HelpTB_MouseEnter(object sender, MouseEventArgs e)
        {
            HelpTB.Foreground = Brushes.Gray;
        }

        private void HelpTB_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_selectedItem != 4)
            {
                HelpTB.Foreground = Brushes.Black;
            }
        }
        #endregion

        #region Click Handlers
        private void GeneralTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedTab(0);
        }

        private void AppearanceTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedTab(1);
        }

        private void ReadingTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedTab(2);
        }

        private void ResearchTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedTab(3);
        }

        private void HelpTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedTab(4);
        }
        #endregion
        #endregion

        private void CloseWindowGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void DragGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
