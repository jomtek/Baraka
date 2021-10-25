using Baraka.ViewModels.UserControls.Player;
using System.Windows.Controls;

namespace Baraka.Views.UserControls.Player
{
    /// <summary>
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        public PlayerView()
        {
            InitializeComponent();
            DataContext = new PlayerViewModel();
        }

        private void ScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            
        }
    }
}
