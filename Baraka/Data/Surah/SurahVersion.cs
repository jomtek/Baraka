using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data.Surah
{
    [Serializable]
    public class SurahVersion
    {
        public string Language { get; set; }
        public string Edition { get; set; }
        public string[] Verses { get; set; }

        public SurahVersion(string language, string edition, string[] verses)
        {
            Language = language;
            Edition = edition;
            Verses = verses;
        }
    }
}
