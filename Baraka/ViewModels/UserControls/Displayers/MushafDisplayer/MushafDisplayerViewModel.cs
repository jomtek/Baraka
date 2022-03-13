using Baraka.Models.Quran.Mushaf;
using Baraka.Services.Quran.Mushaf;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Baraka.ViewModels.UserControls.Displayers.MushafDisplayer
{
    public class MushafDisplayerViewModel : NotifiableBase
    {
        public MushafPageViewModel LeftPageVm { get; set; }
        public MushafPageViewModel RightPageVm { get; set; }

        private MushafPreloadedPageCache _cache = new();

        private int _defaultLocation = 600;
        private int _cacheRadius = 40;

        private double _fontSize = 13.5;
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                try
                {
                    LeftPageVm.FontSize = value;
                    RightPageVm.FontSize = value;
                    _fontSize = value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                }
            }
        }

        public ICommand TurnPageLeftCommand { get; set; }
        public ICommand TurnPageRightCommand { get; set; }
        public MushafDisplayerViewModel()
        {
            TurnPageLeftCommand = new RelayCommand((param) =>
            {
                SetPage(LeftPageVm.Page + 2);
            });

            TurnPageRightCommand = new RelayCommand((param) =>
            {
                SetPage(LeftPageVm.Page - 2);
            });

            _cache.PageGenerationRequested += Cache_PageGenerationRequested;

            for (int i = _defaultLocation; i >= _defaultLocation - _cacheRadius + 2; i -= 2)
            {
                BuildPage(i, CacheOrientation.FAR_RIGHT);
                BuildPage(i - 1, CacheOrientation.FAR_RIGHT);
            }

            LeftPageVm = new MushafPageViewModel(_defaultLocation, _cache);
            RightPageVm = new MushafPageViewModel(_defaultLocation - 1, _cache);
            FontSize = 13.5;
        }

        private void Cache_PageGenerationRequested(int page, CacheOrientation orientation)
        {
            // Fire and forget 
            Task.Run(() => BuildPage(page, orientation)).ConfigureAwait(false);
        }

        public void SetPage(int page)
        {
            if (page > LeftPageVm.Page)
            {
                // Navigation towards the left
                for (int i = 0; i < 2; i++)
                {
                    _cache.PreloadPage(CacheOrientation.FAR_LEFT);
                    _cache.ErasePage(CacheOrientation.FAR_RIGHT);
                }
            }
            else if (page < LeftPageVm.Page)
            {
                // Navigation towards the right
                for (int i = 0; i < 2; i++)
                {
                    _cache.PreloadPage(CacheOrientation.FAR_RIGHT);
                    _cache.ErasePage(CacheOrientation.FAR_LEFT);
                }
            }

            LeftPageVm.SetPage(page);
            RightPageVm.SetPage(page - 1);

            var pp = "";
            foreach (var key in _cache.Stack)
                pp += key.Page + ",";
            Trace.WriteLine(pp);
        }

        private Task BuildPage(int page, CacheOrientation orientation)
        {
            if (_cache.HasPage(page))
                return Task.CompletedTask;

            Trace.WriteLine($"creating lines for page {page}");

            var stackpanels = new List<StackPanel>();

            var glyphs = MushafGlyphService.RetrievePage(page).ToList();
            foreach (var line in glyphs)
            {
                var childSP = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                };

                foreach (var glyph in line.Glyphs)
                {
                    var control = new Glyphs()
                    {
                        Fill = Brushes.Black,
                        FontRenderingEmSize = FontSize,
                        UnicodeString = glyph.DecodedData.ToString(),
                    };

                    if (glyph.Type == MushafGlyphType.BASMALA || glyph.Type == MushafGlyphType.SURA_NAME)
                        control.FontUri = new Uri(MushafFontService.FindPageFontName(0, true));
                    else
                        control.FontUri = new Uri(MushafFontService.FindPageFontName(glyph.Page, true));

                    childSP.Children.Add(control);
                }

                childSP.Measure(new Size(200, 25));
                childSP.Arrange(new Rect(new Size(200, 25)));
                //childSP.SizeChanged += ChildSP_SizeChanged;

                stackpanels.Add(childSP);
                //LinesSP.Children.Add(childSP);
            }

            _cache.AddPage(page, stackpanels, orientation);

            return Task.CompletedTask;
        }
    }
}
