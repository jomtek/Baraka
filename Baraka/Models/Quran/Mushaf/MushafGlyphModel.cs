using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models.Quran.Mushaf
{
    [Serializable]
    public enum MushafGlyphType
    {
        WORD,
        END_OF_AYAH,
        RUB_EL_HIZB,
        SUJOOD,
        STOPPING_SIGN,
        SURA_NAME,
        BASMALA,
    }

    [Serializable]
    public class MushafGlyphModel
    {
        public char DecodedData { get; } // Unicode glyph
        public VerseLocationModel AssociatedVerse { get; }
        public MushafGlyphType Type { get; }
        public int Page { get; }

        public MushafGlyphModel(char decodedData, VerseLocationModel associatedVerse, MushafGlyphType type, int page)
        {
            DecodedData = decodedData;
            AssociatedVerse = associatedVerse;
            Type = type;
            Page = page;
        }
    }
}
