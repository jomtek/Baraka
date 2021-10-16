using Baraka.ViewModels.Splashes;
using Baraka.Views;
using Baraka.Views.Splashes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            var splashView = new WelcomeWindow()
            {
                DataContext = splashVm,
            };

            // Initialize the main window
            
            MainWindow = new MainWindow()
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
