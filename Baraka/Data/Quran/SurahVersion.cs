using System;

namespace Baraka.Data.Surah
{
    [Serializable]
    public class SurahVersion
    {
        public string Language { get; set; }
        public string[] Verses { get; set; }

        public SurahVersion(string language, string[] verses)
        {
            Language = language;
            Verses = verses;
        }
    }
}
