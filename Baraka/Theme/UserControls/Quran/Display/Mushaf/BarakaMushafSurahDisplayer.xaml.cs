using Baraka.Data;
using Baraka.Data.Descriptions;
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

namespace Baraka.Theme.UserControls.Quran.Display.Mushaf
{
    /// <summary>
    /// Logique d'interaction pour BarakaMushafSurahDisplayer.xaml
    /// </summary>
    public partial class BarakaMushafSurahDisplayer : UserControl, ISurahDisplayer
    {
        public BarakaMushafSurahDisplayer()
        {
            InitializeComponent();
        }

        private void Run_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Run)sender).Foreground = Brushes.Yellow;
        }

        private void Run_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Run)sender).Foreground = Brushes.Black;
        }

        public async Task LoadSurahAsync(SurahDescription surah)
        {
            throw new NotImplementedException();
        }

        public void UnloadActualSurah()
        {
        }

        private void LeftPage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Debug purposes
            LeftPage.LoadPage(1);
        }
    }
}
