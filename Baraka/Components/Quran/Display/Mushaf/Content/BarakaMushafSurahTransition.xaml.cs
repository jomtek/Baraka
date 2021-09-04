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

namespace Baraka.Theme.UserControls.Quran.Display.Mushaf.Content
{
    /// <summary>
    /// Logique d'interaction pour BarakaMushafSurahTransition.xaml
    /// </summary>
    public partial class BarakaMushafSurahTransition : UserControl
    {
        // `suraName` must be provided already decoded
        public BarakaMushafSurahTransition(string suraName, VerseDescription associatedVerse)
        {
            InitializeComponent();
            SuraNameTB.Text = suraName;
            SuraNameVB.ToolTip = Utils.Quran.General.GenerateSynopsis(associatedVerse.Surah);
        }

        #region Effects
        private void Viewbox_MouseEnter(object sender, MouseEventArgs e)
        {
            SuraNameTB.Foreground = (SolidColorBrush)App.Current.Resources["MediumBrush"];
        }
        private void Viewbox_MouseLeave(object sender, MouseEventArgs e)
        {
            SuraNameTB.Foreground = Brushes.Black;
        }
        #endregion

    }
}
