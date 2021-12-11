using Baraka.Services.Quran;
using Baraka.Singletons;
using Baraka.Stores;
using Baraka.ViewModels;
using Baraka.ViewModels.Splashes;
using Baraka.Views;
using Baraka.Views.Splashes;
using System.Globalization;
using System.Threading;
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

            Services.Quran.Mushaf.MushafGlyphService.LoadGlyphsFromFile("glyph_info.bkser");
            System.Diagnostics.Trace.WriteLine("Glyphs loaded from `glyph_info.bkser` !");

            // Initialize the singletons
            AppStateSingleton.Instance.CurrentlyDisplayedSura = SuraInfoService.FromNumber(1);

            // Initialize the splash screen
            var splashVm = new WelcomeViewModel();
            var splashView = new WelcomeView()
            {
                DataContext = splashVm,
            };

            // Initialize the stores
            var selectedSuraStore = new SelectedSuraStore();

            // Initialize the main window
            var mainVm = new MainViewModel(selectedSuraStore);
            MainWindow = new MainView()
            {
                DataContext = mainVm,
            };

            selectedSuraStore.ChangeSelectedSura(SuraInfoService.FromNumber(1));

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
