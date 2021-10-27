using Baraka.ViewModels.UserControls.Player;
using Baraka.Views.UserControls.Player.Pages;
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
    }
}
