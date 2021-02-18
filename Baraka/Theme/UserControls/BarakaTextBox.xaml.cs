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
    /// Logique d'interaction pour BarakaTextBox.xaml
    /// </summary>
    public partial class BarakaTextBox : UserControl
    {
        public BarakaTextBox()
        {
            InitializeComponent();
        }

        private void ImageComponent_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ImageComponent.IsMouseOver)
            {
                ImageComponent.Opacity = 0.7;
                Cursor = Cursors.Hand;
            }
            else
            {
                ImageComponent.Opacity = 1;
                Cursor = Cursors.Arrow;
            }
        }

        private void TextBoxComponent_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxComponent.Opacity == 0.65)
            {
                TextBoxComponent.Clear();
                TextBoxComponent.Opacity = 1;
            }
        }

        private void TextBoxComponent_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxComponent.Text.Trim().Length == 0)
            {
                TextBoxComponent.Text = "rechercher...";
                TextBoxComponent.Opacity = 0.65;
            }
        }
    }
}
