using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data.Surah
{
    [Serializable]
    public class SurahVersionConfig : ICloneable, IEquatable<SurahVersionConfig>
    {
        public bool DisplayArabic { get; set; }
        public bool DisplayPhonetic { get; set; }
        public bool DisplayTranslated { get; set; }
        public int Translation1 { get; set; }
        public int Translation2 { get; set; }
        public int Translation3 { get; set; }

        public SurahVersionConfig(
            bool displayArabic,
            bool displayPhonetic,
            bool displayTranslated,
            int translation1,
            int translation2,
            int translation3)
        {
            DisplayArabic = displayArabic;
            DisplayPhonetic = displayPhonetic;
            DisplayTranslated = displayTranslated;
            Translation1 = translation1;
            Translation2 = translation2;
            Translation3 = translation3;
        }

        public bool ShowMushaf()
        {
            return DisplayArabic && !DisplayPhonetic && !DisplayTranslated;
        }

        public object Clone()
        {
            return new SurahVersionConfig(DisplayArabic, DisplayPhonetic, DisplayTranslated, Translation1, Translation2, Translation3);
        }

        public bool Equals(SurahVersionConfig other)
        {
            return DisplayArabic == other.DisplayArabic &&
                   DisplayPhonetic == other.DisplayPhonetic &&
                   DisplayTranslated == other.DisplayTranslated &&
                   Translation1 == other.Translation1 &&
                   Translation2 == other.Translation2 &&
                   Translation3 == other.Translation3;
        }
    }
}
