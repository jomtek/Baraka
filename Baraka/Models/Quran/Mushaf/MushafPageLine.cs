using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models.Quran.Mushaf
{
    public class MushafPageLine
    {
        public MushafGlyphModel[] Glyphs { get; private set; }
        public MushafPageLine(MushafGlyphModel[] glyphs)
        {
            Glyphs = glyphs;
        }
    }
}
