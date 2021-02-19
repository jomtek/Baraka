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

namespace Baraka
{
    /// <summary>
    /// Logique d'interaction pour QuranTranslationsManagerWindow.xaml
    /// </summary>
    public partial class QuranTranslationsManagerWindow : Window
    {
        public QuranTranslationsManagerWindow()
        {
            InitializeComponent();
        }

        private void CloseFormCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void CloseFormCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
            CloseFormPath.Fill = new SolidColorBrush(Color.FromRgb(222, 222, 222));
        }

        private void CloseFormCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            CloseFormPath.Fill = Brushes.White;
        }
    }
}
