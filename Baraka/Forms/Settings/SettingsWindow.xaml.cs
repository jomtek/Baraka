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
            };
            FrameComponent.Content = _pages[0];
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            //this.Close();
        }

        #region Dashboard
        // FrameComponent.Content = new GeneralPage();
        private int _selectedItem = 0;

        private void SetSelectedItem(int item)
        {
            switch (item)
            {
                case 0:
                    GeneralTB.Foreground = Brushes.Gray;
                    AppearanceTB.Foreground = Brushes.Black;
                    ReadingTB.Foreground = Brushes.Black;
                    ResearchTB.Foreground = Brushes.Black;
                    break;
                case 1:
                    AppearanceTB.Foreground = Brushes.Gray;
                    GeneralTB.Foreground = Brushes.Black;
                    ReadingTB.Foreground = Brushes.Black;
                    ResearchTB.Foreground = Brushes.Black;
                    break;
                case 2:
                    ReadingTB.Foreground = Brushes.Gray;
                    AppearanceTB.Foreground = Brushes.Black;
                    GeneralTB.Foreground = Brushes.Black;
                    ResearchTB.Foreground = Brushes.Black;
                    break;
                case 3:
                    ResearchTB.Foreground = Brushes.Gray;
                    ReadingTB.Foreground = Brushes.Black;
                    AppearanceTB.Foreground = Brushes.Black;
                    GeneralTB.Foreground = Brushes.Black;
                    break;
            }

            _selectedItem = item;
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
        #endregion

        #region Click Handlers
        private void GeneralTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedItem(0);
            FrameComponent.Content = _pages[0];
        }

        private void AppearanceTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedItem(1);
            FrameComponent.Content = _pages[1];
        }

        private void ReadingTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedItem(2);
            FrameComponent.Content = _pages[2];
        }


        private void ResearchTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetSelectedItem(3);
            FrameComponent.Content = _pages[3];
        }
        #endregion
        #endregion

        private void CloseWindowGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void CloseWindowGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseWindowPath.Fill = Brushes.Gray;
        }

        private void CloseWindowGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseWindowPath.Fill = Brushes.Black;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void DragGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
