using Baraka.Behaviors;
using Baraka.ViewModels.UserControls.Player.Pages;
using System.Windows.Controls;
using System.Windows.Input;

namespace Baraka.Views.UserControls.Player.Pages
{
    /// <summary>
    /// Interaction logic for QariTabView.xaml
    /// </summary>
    public partial class QariTabView : UserControl
    {
        public QariTabView()
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
