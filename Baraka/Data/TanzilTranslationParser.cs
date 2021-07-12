using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data
{
    class TanzilTranslationParser
    {
        public List<string[]> Result; // 114 desired entries
        public TanzilTranslationParser(string raw)
        {
            Result = new List<string[]>();
            Go(raw);
        }

        private void Go(string raw)
        {
            int bufferSurah = 1;
            var verseBuffer = new List<string>();

            foreach (string verse in raw.Split('\n'))
            {
                if (!verse.Contains('|')) // Get rid of the end messages
                    continue;

                int surahNum = Convert.ToInt32(verse.Substring(0, verse.IndexOf('|')));

                if (surahNum != bufferSurah)
                {
                    Result.Add(verseBuffer.ToArray());
                    verseBuffer.Clear();
                    bufferSurah = surahNum;
                }

                string cleanVerse = verse.Substring(verse.LastIndexOf('|') + 1);
                verseBuffer.Add(cleanVerse);
            }

            // Last surah (An-Nas)
            Result.Add(verseBuffer.ToArray());
        }
    }
}
