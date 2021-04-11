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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Baraka.Theme.UserControls.Dashboard
{
    /// <summary>
    /// Logique d'interaction pour BarakaDashboardItem.xaml
    /// </summary>
    public partial class BarakaDashboardItem : UserControl
    {
        #region Settings
        public int ItemIndex
        {
            set
            {
                switch (value)
                {
                    // TODO: globalization

                    case 0: // Research
                        ItemTBL.Text = "Rechercher";
                        ItemICON.Style = (Style)FindResource("Search");
                        break;
                    case 1: // Library
                        ItemTBL.Text = "Bibliothèque";
                        ItemICON.Style = (Style)FindResource("Library");
                        break;
                    case 2: // Settings
                        ItemTBL.Text = "Paramètres";
                        ItemICON.Style = (Style)FindResource("Cog");
                        break;
                }
            }
        }
        #endregion

        public BarakaDashboardItem()
        {
            InitializeComponent();

        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            var brush = new SolidColorBrush((Color)FindResource(SystemColors.ActiveBorderColorKey));
            ItemICON.Fill = brush;

            Cursor = Cursors.Hand;

            Console.WriteLine("hellooooooooooo");

            ((Storyboard)this.Resources["MouseEnterStory"]).Begin();
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            ItemICON.Fill = Brushes.White;

            Cursor = Cursors.Arrow;

            ((Storyboard)this.Resources["MouseLeaveStory"]).Begin();
        }
    }
}
