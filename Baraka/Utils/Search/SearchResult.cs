using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using Baraka.Data;

namespace Baraka.Utils.Search
{
    public class SearchResult
    {
        public SurahDescription Surah { get; private set; }
        public int Verse { get; private set; }
        public string[] Terms { get; private set; }

        public SearchResult(SurahDescription surah, int verse, string[] terms)
        {
            Surah = surah;
            Verse = verse;
            Terms = terms;
        }
    }
}
