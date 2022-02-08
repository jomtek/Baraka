using Baraka.Models;
using Baraka.Models.State;
using Baraka.Services;
using Baraka.Services.Quran;
using Baraka.ViewModels;
using Baraka.ViewModels.Splashes;
using Baraka.ViewModels.UserControls.Displayers.TextDisplayer;
using Baraka.Views;
using Baraka.Views.Splashes;
using System.Globalization;
using System.Threading;
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

            // Debugging purposes
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");

            // Uncomment these two lines to generate glyph info
            //Services.Quran.Mushaf.MushafGlyphService.GenerateGlyphInfo();
            //Services.Quran.Mushaf.MushafGlyphService.SaveGlyphsToPath("glyph_info.bks");

            Services.Quran.Mushaf.MushafGlyphService.LoadGlyphsFromFile("glyph_info.bks");
            System.Diagnostics.Trace.WriteLine("Glyphs loaded from `glyph_info.bkser` !");

            // Initialize the splash screen
            var splashVm = new WelcomeViewModel();
            var splashView = new WelcomeView()
            {
                DataContext = splashVm,
            };

            // Initialize fundamental states
            var app = AppState.Create();
            var bookmark = BookmarkState.Create();

            // Initialize the main window
            MainWindow = new MainView()
            {
                DataContext = MainViewModel.Create(app, bookmark),
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