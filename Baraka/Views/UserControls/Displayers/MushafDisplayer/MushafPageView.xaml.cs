using Baraka.Models.Quran.Mushaf;
using Baraka.Services.Quran.Mushaf;
using Baraka.ViewModels.UserControls.Displayers.MushafDisplayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Baraka.Views.UserControls.Displayers.MushafDisplayer
{
    /// <summary>
    /// Interaction logic for MushafPageView.xaml
    /// </summary>
    public partial class MushafPageView : ContentControl
    {
        private MushafPageViewModel _vm;

        public MushafPageView()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is MushafPageViewModel vm)
            {
                _vm = vm;
                _vm.DisplayRequested += (page) => MushafPageViewModel_DisplayRequested(page);
                _vm.PageWidthChanged += (width) =>
                {
                    ContainerGrid.Width = width;
                };
            }
        }

        private int _currentPage;
        private async void MushafPageViewModel_DisplayRequested(int page, bool fastMode = false)
        {
            Trace.WriteLine($"page width: {ContainerGrid.ActualWidth}");
            if (_vm == null)
                throw new ArgumentException();

            // Make sure the pages don't mix each other
            _currentPage = page;

            LinesSP.Children.Clear();

            if (!fastMode)
            {
                SV.Visibility = Visibility.Collapsed;
                LoadingLBL.Visibility = Visibility.Visible;
                LoadingLBL.Content = $"page {page}";
            }

            await Task.Delay(5); // Invalidate layout
            var lines = await BuildLines(page);
            if (!fastMode) await Task.Delay(100); // Invalidate layout

            if (_currentPage == page)
            {
                if (!fastMode)
                {
                    SV.Visibility = Visibility.Visible;
                    LoadingLBL.Visibility = Visibility.Hidden;
                }

                foreach (StackPanel line in lines)
                    LinesSP.Children.Add(line);
            }
        }

        private Task<List<StackPanel>> BuildLines(int page)
        {
            var stackpanels = new List<StackPanel>();

            if (page < 0)
            {
                Trace.WriteLine("returning empty list");
                return Task.FromResult(stackpanels);
            }

            var glyphs = MushafGlyphService.RetrievePage(page).ToList();
            foreach (var line in glyphs)
            {
                var childSP = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };

                foreach (var glyph in line.Glyphs)
                {
                    var control = new Glyphs()
                    {
                        Fill = Brushes.Black,
                        FontRenderingEmSize = _vm.FontSize,
                        UnicodeString = glyph.DecodedData.ToString(),
                    };

                    control.DataContext = _vm;
                    control.SetBinding(
                        Glyphs.FontRenderingEmSizeProperty,
                        new Binding("FontSize")
                    );

                    if (glyph.Type == MushafGlyphType.BASMALA || glyph.Type == MushafGlyphType.SURA_NAME)
                        control.FontUri = new Uri(MushafFontService.FindPageFontName(0, true));
                    else
                        control.FontUri = new Uri(MushafFontService.FindPageFontName(glyph.Page, true));

                    childSP.Children.Add(control);
                }

                childSP.Measure(new Size(200, 25));
                childSP.Arrange(new Rect(new Size(200, 25)));

                stackpanels.Add(childSP);
            }

            return Task.FromResult(stackpanels);
        }

        private void ChildSP_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Trace.WriteLine($"new size is {e.NewSize}");
        }
    }
}
