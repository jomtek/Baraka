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
using System.ComponentModel;

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaMarkbook.xaml
    /// </summary>
    public partial class BarakaBookmark : UserControl
    {
        [Category("Baraka")]
        public event EventHandler OnPlanetClick;

        public BarakaBookmark()
        {
            InitializeComponent();
        }

        #region Planet Button
        private void PlanetBTN_MouseEnter(object sender, MouseEventArgs e)
        {
            PlanetPath.StrokeThickness = 1.5;
            PlanetPath.Stroke = (SolidColorBrush)App.Current.Resources["LightBrush"];
        }

        private void PlanetBTN_MouseLeave(object sender, MouseEventArgs e)
        {
            PlanetPath.StrokeThickness = 1.25;
            PlanetPath.Stroke = (SolidColorBrush)App.Current.Resources["DarkBrush"];
        }
        #endregion

        private void PlanetBTN_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnPlanetClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
