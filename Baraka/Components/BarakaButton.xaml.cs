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

namespace Baraka.Theme.UserControls
{
    /// <summary>
    /// Logique d'interaction pour BarakaButton.xaml
    /// </summary>
    public partial class BarakaButton : UserControl
    {
        public BarakaButton()
        {
            InitializeComponent();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            border.Background = (SolidColorBrush)FindResource("HoveredBrush");
            dropShadow.Opacity = 0.45;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            border.Background = (SolidColorBrush)FindResource("DarkBrush");
            dropShadow.Opacity = 0.4;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            border.Background = (SolidColorBrush)FindResource("PressedBrush");
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            border.Background = (SolidColorBrush)FindResource("HoveredBrush");
        }
    }
}
