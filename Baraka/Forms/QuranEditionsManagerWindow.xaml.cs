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

namespace Baraka.Forms
{
    /// <summary>
    /// Logique d'interaction pour QuranEditionsManager.xaml
    /// </summary>
    public partial class QuranEditionsManagerWindow : Window
    {
        public QuranEditionsManagerWindow()
        {
            InitializeComponent();
        }

        #region Display position
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MoveBottomRightEdgeOfWindowToMousePosition();
        }

        // SOF (982639/alexandru)
        private void MoveBottomRightEdgeOfWindowToMousePosition()
        {
            var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            var mouse = transform.Transform(Utils.General.GetMousePositionWindowsForms());
            Left = mouse.X;
            Top = mouse.Y;
        }
        #endregion
    }
}
