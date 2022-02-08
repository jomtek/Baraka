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
        }

        private void UC_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is PlayerViewModel vm)
            {
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
}
