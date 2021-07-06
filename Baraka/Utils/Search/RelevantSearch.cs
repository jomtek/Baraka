using Baraka.Data;
using Baraka.Data.Descriptions;
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
            search = search.ToLower(); // TODO

            string[] keywords = search.Split(' ');

            var results = new List<SearchResult>();
            var founds = new List<(int, int)>(); // Surah number, verse number

            // First try, match exact keyword order
            foreach (var key in LoadedData.SurahList)
            {
                var surah = key.Key;
                if ( !KeepSurah(surah) )
                {
                    continue;
                }
                
                /* CHANGED
                string[] verses = key.Value[2].Verses;

                for (int i = 0; i < verses.Length; i++)
                {
                    if (verses[i].ToLower().Contains(search))
                    {
                        results.Add(new SearchResult(surah, i, new string[1] { search }));
                        founds.Add((surah.SurahNumber, i));
                    }
                }
                */
            }

            // Second try, match keywords in any order
            foreach (var key in LoadedData.SurahList)
            {
                var surah = key.Key;
                if (!KeepSurah(surah))
                {
                    continue;
                }

                //return;
                /*

                string[] verses = key.Value[2].Verses;

                for (int i = 0; i < verses.Length; i++)
                {
                    if (founds.Contains((surah.SurahNumber, i))) continue;
                    if (keywords.All(verses[i].ToLower().Contains))
                    {
                        results.Add(new SearchResult(key.Key, i, keywords));
                    }
                }
                */
            }

            return results;
        } 
    }
}
