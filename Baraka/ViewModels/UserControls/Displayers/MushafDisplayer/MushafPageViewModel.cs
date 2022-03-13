using Baraka.Models.Quran;
using Baraka.Models.Quran.Mushaf;
using Baraka.Services.Quran.Mushaf;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Baraka.ViewModels.UserControls.Displayers.MushafDisplayer
{
    public class MushafPageViewModel : NotifiableBase
    {
        public event Action<List<StackPanel>> DisplayRequested;
        private MushafPreloadedPageCache _cache;

        public int Page { get; private set; }

        private double _fontSize;
        public double FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; OnPropertyChanged(nameof(FontSize)); }
        }

        private VerseLocationModel? _hoveredVerse;
        public VerseLocationModel? HoveredVerse
        {
            get { return _hoveredVerse; }
            set { _hoveredVerse = value; OnPropertyChanged(nameof(HoveredVerse)); }
        }

        public ICommand GlyphHoveredCommand { get; set; }
        public ICommand GlyphUnhoveredCommand { get; set; }
        public MushafPageViewModel(int page, MushafPreloadedPageCache cache)
        {
            _cache = cache;

            SetPage(page);

            GlyphHoveredCommand = new RelayCommand((param) =>
            {
                if (param is MushafGlyphModel glyph && glyph.Type == MushafGlyphType.END_OF_AYAH)
                {
                    HoveredVerse = glyph.AssociatedVerse;
                }
            });

            GlyphUnhoveredCommand = new RelayCommand((param) =>
            {
                HoveredVerse = null;
            });
        }

        public void SetPage(int page)
        {
            try
            {
                DisplayRequested?.Invoke(_cache.GetPage(page).Items);
                Page = page;
            } catch (Exception ex)
            {
                Console.WriteLine();
            }
        }
    }
}
