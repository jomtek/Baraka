using Baraka.ViewModels.Splashes;
using Baraka.Views;
using Baraka.Views.Splashes;
using System.Windows;

namespace Baraka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize the splash screen
            var splashVm = new WelcomeViewModel();
            var splashView = new WelcomeView()
            {
                DataContext = splashVm,
            };

            // Initialize the main window
            
            MainWindow = new MainView()
            {

            };

            // Pop-corn time !
            splashVm.ClosingRequest += () =>
            {
                splashView.Close();
                MainWindow.Show();
            };

            splashView.Show();



        }
    }
}
