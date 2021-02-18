using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data.Descriptions
{
    [Serializable]
    public class SurahDescription
    {
        public int SurahNumber { get;  set; }
        public int NumberOfVerses { get; set; }
        public string PhoneticName { get; set; }
        public string TranslatedName { get; set; } // todo: globalization
        public SurahRevelationType RevelationType { get; set; }

        public SurahDescription()
        {
            SurahNumber = 1;
            NumberOfVerses = 7;
            PhoneticName = "Prologue";
            TranslatedName = "Al Fatiha";
            RevelationType = SurahRevelationType.M;
        }

        public SurahDescription(
            int num,
            int versesCount,
            string phoneticName,
            string translatedName,
            SurahRevelationType revelation)
        {
            SurahNumber = num;
            NumberOfVerses = versesCount;
            PhoneticName = phoneticName;
            TranslatedName = translatedName;
            RevelationType = revelation;
        }
    }
}
