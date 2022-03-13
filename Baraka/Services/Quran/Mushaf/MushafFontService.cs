using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Baraka.Services.Quran.Mushaf
{
    public static class MushafFontService
    {
        // All the fonts belong to the Saudi King Fahd complex and were edited to suit Baraka's needs
        private static readonly string _fontsPath;

        private static Dictionary<int, FontFamily> _fontsCache = new();

        static MushafFontService()
        {
            var api = (string)App.Current.FindResource("API_PATH");
            // TODO: adapt to the online api
            _fontsPath = $@"{api}\mushaf\fonts";
        }

        // `page` starts from 1
        public static string FindPageFontName(int page, bool getPath = false)
        {
            string fontName;
            if (page == 0)
            {
                fontName = "QCF_BSML";
            }
            else
            {
                fontName = $"QCF_P{page.ToString("000")}";
            }

            if (getPath)
                return $@"{_fontsPath}\{fontName}.TTF";
            else
                return fontName;
        }

        // `page` starts from 1
        public static FontFamily FindPageFontFamily(int page)
        {
            if (_fontsCache.ContainsKey(page))
                return _fontsCache[page];

            var uri = $@"{_fontsPath}\#{FindPageFontName(page)}";
            
            var family = new FontFamily(uri);
            _fontsCache.Add(page, family);

            return family;
        }
    }
}
