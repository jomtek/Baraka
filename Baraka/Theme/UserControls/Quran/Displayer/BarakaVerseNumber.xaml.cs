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

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaVerseNumber.xaml
    /// </summary>
    public partial class BarakaVerseNumber : UserControl
    {
        public BarakaVerseNumber()
        {
            InitializeComponent();
        }
        public BarakaVerseNumber(int number)
        {
            InitializeComponent();
            NumberTB.Text = number.ToString();
        }
    }
}
