using Baraka.ViewModels.Splashes;
using System.Windows;
using System.Windows.Input;

namespace Baraka.Views.Splashes
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : Window
    {
        public WelcomeView()
        {
            InitializeComponent();
            DataContext = new WelcomeViewModel();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
