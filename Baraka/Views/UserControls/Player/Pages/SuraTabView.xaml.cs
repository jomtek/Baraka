using Baraka.Behaviors;
using Baraka.ViewModels.UserControls.Player.Pages;
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

namespace Baraka.Views.UserControls.Player.Pages
{
    /// <summary>
    /// Interaction logic for SuraTabView.xaml
    /// </summary>
    public partial class SuraTabView : UserControl
    {
        public SuraTabView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer && scrollViewer.ScrollableHeight > 0)
            {
                double state = scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight;
                ScrollViewerBehavior.SetScrollState(scrollViewer, state);
            }
        }
    }
}
