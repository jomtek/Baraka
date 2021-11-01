using Baraka.Behaviors;
using Baraka.ViewModels.UserControls.Displayers;
using Baraka.ViewModels.UserControls.Displayers.TextDisplayer;
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

namespace Baraka.Views.UserControls.Displayers.TextDisplayer
{
    /// <summary>
    /// Interaction logic for TextDisplayerView.xaml
    /// </summary>
    public partial class TextDisplayerView : UserControl
    {
        public TextDisplayerView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                double state = scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight;
                ScrollViewerBehavior.SetScrollState(scrollViewer, state);
            }
        }
    }
}
