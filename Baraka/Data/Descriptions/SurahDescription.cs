using System;

namespace Baraka.Data.Descriptions
{
    [Serializable]
    public class SurahDescription
    {
        public int SurahNumber { get; set; }
        public int NumberOfVerses { get; set; }
        public string PhoneticName { get; set; }
        public string TranslatedName { get; set; } // todo: globalization
        public SurahRevelationType RevelationType { get; set; }

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

        public bool HasBasmala()
        {
            // Exclude Al-Fatiha and At-Tawba
            return SurahNumber != 1 && SurahNumber != 9;
        }
    }
}
