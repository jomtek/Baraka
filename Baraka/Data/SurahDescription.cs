using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data
{
    public class SurahDescription
    {
        public int SurahNumber { get; private set; }
        public int NumberOfVerses { get; private set; }
        public string PhoneticName { get; private set; }
        public string TranslatedName { get; private set; } // todo: globalization
        public SurahRevelationType RevelationType { get; private set; }

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
