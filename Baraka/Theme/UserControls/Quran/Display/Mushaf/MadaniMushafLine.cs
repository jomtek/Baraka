using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Theme.UserControls.Quran.Display.Mushaf
{
    // TODO: think about moving this to Data/Quran
    public class MadaniMushafLine
    {
        public int page { get; set; }
        public int line { get; set; }
        public int sura { get; set; }
        public int ayah { get; set; }
        public string text { get; set; }
    }
}
