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

namespace Baraka.Components.Quran.Display.Mushaf.Content
{
    /// <summary>
    /// Logique d'interaction pour BarakaMushafEndOfAyah.xaml
    /// </summary>
    public partial class BarakaMushafEndOfAyah : Run
    {
        private void Run_MouseEnter(object sender, MouseEventArgs e)
        {
            Background = Brushes.Red;
        }

        private void Run_MouseLeave(object sender, MouseEventArgs e)
        {
            Background = Brushes.Transparent;
        }
    }
}
