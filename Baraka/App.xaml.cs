using Baraka.Services;
using Baraka.Services.Quran;
using Baraka.Singletons;
using Baraka.ViewModels;
using Baraka.ViewModels.Splashes;
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

            // Initialize the singletons
            AppStateSingleton.Instance.SelectedSuraStore.Value = SuraInfoService.FromNumber(1);

            // Initialize the splash screen
            var splashVm = new WelcomeViewModel();
            var splashView = new WelcomeView()
            {
                DataContext = splashVm,
            };

            // Initialize the main window
            var mainVm = new MainViewModel();
            MainWindow = new MainView()
            {
                DataContext = mainVm,
            };

            AppStateSingleton.Instance.SelectedSuraStore.Value = SuraInfoService.FromNumber(2);
            AppStateSingleton.Instance.SelectedQariStore.Value = QariInfoService.GetAll()[0];

            System.Console.WriteLine();
            // Pop-corn time !
            splashVm.ClosingRequest += () =>
            {
                splashView.Close();
                MainWindow.Show();
            };

            splashView.Show();

            Task.Run(PlayMonitor.StartMonitoring).ConfigureAwait(false);
            
        }
    }
}
