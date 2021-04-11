using System;

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
