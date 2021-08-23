using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.IO;
using Baraka.Theme.UserControls.Quran.Display.Mushaf;
using System.Data;
using System.Data.SQLite;
using Dapper;
using Baraka.Data.Descriptions;

namespace Baraka.Data.Quran.Mushaf
{
    public class MushafFontManager
    {
        // All the fonts belong to the Saudi King Fahd complex and were edited to suit Baraka's needs
        private string _fontsPath;

        public MushafFontManager()
        {
            _fontsPath =
                Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName +
                @"\data\quran\mushaf\fonts";
        }

        // `page` starts from 1
        public FontFamily FindPageFontFamily(int page)
        {
            string fontName;
            if (page == 0)
            {
                fontName = "QCF_BSML";
            }
            else
            {
                fontName = $"QCF_P{(page).ToString("000")}";
            }

            return new FontFamily($@"{_fontsPath}\#{fontName}");
        }  
    }
}
