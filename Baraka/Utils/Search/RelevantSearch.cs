using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Utils.Search
{
    public class RelevantSearch
    {
        private SurahRevelationType _revelation;
        private int _surahNum;

        public RelevantSearch(SurahRevelationType revelation, int surahNum)
        {
            _revelation = revelation;
            _surahNum = surahNum;
        }

        private bool KeepSurah(SurahDescription surah)
        {
            return (_surahNum == 0 || surah.SurahNumber == _surahNum) &&
                   (_revelation == SurahRevelationType.MH || surah.RevelationType == _revelation);
        }

        public List<SearchResult> Go(string search)
        {
            string[] keywords = search.Split(' ');

            var results = new List<SearchResult>();
            var firstStepFounds = new List<(int, int)>(); // Surah number, verse number

            foreach (var entry in LoadedData.SurahList)
            {
                SurahDescription surah = entry.Key;

                if (!KeepSurah(surah))
                {
                    continue;
                }

                var edition = entry.Value[LoadedData.Settings.SearchEdition];

                // First try, match with exact keywords order
                for (int i = 0; i < edition.Verses.Length; i++)
                {
                    if (edition.Verses[i].ToLower().Contains(search))
                    {
                        results.Add(new SearchResult(surah, i, new string[1] { search }));
                        firstStepFounds.Add((surah.SurahNumber, i));
                    }
                }

                // Second try, match keywords in any order
                for (int i = 0; i < edition.Verses.Length; i++)
                {
                    if (firstStepFounds.Contains((surah.SurahNumber, i)))
                    {
                        continue;
                    }
                    else if (keywords.All(edition.Verses[i].ToLower().Contains))
                    {
                        results.Add(new SearchResult(surah, i, keywords));
                    }
                }
            }

            return results;
        } 
    }
}
