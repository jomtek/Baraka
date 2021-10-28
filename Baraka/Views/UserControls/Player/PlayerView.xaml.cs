using Baraka.ViewModels.UserControls.Player;
using Baraka.Views.UserControls.Player.Pages;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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

            var vm = new PlayerViewModel();
            DataContext = vm;

            vm.PlayerOpenChanged += (open) =>
            {
                if (open)
                {
                    ((Storyboard)FindResource("OpenPlayerStory")).Begin();
                }
                else
                {
                    ((Storyboard)FindResource("ClosePlayerStory")).Begin();
                }
            };
        }
    }
}
