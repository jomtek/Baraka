using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data.Descriptions
{
    public class VerseDescription
    {
        public SurahDescription Surah { get; set; }
        public int Number { get; set; }

        public VerseDescription(SurahDescription surah, int number)
        {
            Surah = surah;
            Number = number;
        }
    }
}
