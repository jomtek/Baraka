using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data.Quran.Mushaf
{
    public class MushafDbQuery
    {
        public int page { get; set; }
        public int sura { get; set; }
        public int ayah { get; set; }
        public string text { get; set; }
        public int line { get; set; } = -1; // Optional
    }
}
