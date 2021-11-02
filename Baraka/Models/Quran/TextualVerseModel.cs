using Baraka.Models.Quran.Mushaf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models.Quran
{
    public class TextualVerseModel
    {
        public VerseLocationModel Location { get; set; }
        public int Number => Location.Number;
        public VerseEditionModel<MushafGlyphModel[]> Arabic { get; set; }
        public VerseEditionModel<string> Transliteration { get; set; }
        public VerseEditionModel<string> Translation1 { get; set; }
        public VerseEditionModel<string> Translation2 { get; set; }
        public VerseEditionModel<string> Translation3 { get; set; }

        public TextualVerseModel()
        {
            Arabic = new VerseEditionModel<MushafGlyphModel[]>(false, null);
            Transliteration = new VerseEditionModel<string>(false, null);
            Translation1 = new VerseEditionModel<string>(false, null);
            Translation2 = new VerseEditionModel<string>(false, null);
            Translation3 = new VerseEditionModel<string>(false, null);
        }
    }
}
