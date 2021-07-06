using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data.Surah
{
    public class SurahVersionConfig
    {
        public bool DisplayArabic { get; set; }
        public bool DisplayPhonetic { get; set; }
        public TranslationDescription Translation1 { get; set; }
        public TranslationDescription Translation2 { get; set; }
        public TranslationDescription Translation3 { get; set; }

        public SurahVersionConfig(
            bool displayArabic,
            bool displayPhonetic,
            TranslationDescription translation1,
            TranslationDescription translation2,
            TranslationDescription translation3)
        {
            DisplayArabic = displayArabic;
            DisplayPhonetic = displayPhonetic;
            Translation1 = translation1;
            Translation2 = translation2;
            Translation3 = translation3;
        }
    }
}
