using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data.Surah
{
    public class HizbDescription : IEquatable<HizbDescription>
    {
        public int StartSurah { get; private set; }
        public int EndSurah { get; private set; }
        public int StartVerse { get; private set; }
        public int EndVerse { get; private set; }

        public int Number { get; private set; }

        public HizbDescription(
            int startSurah, int endSurah,
            int startVerse, int endVerse,
            int number)
        {
            StartSurah = startSurah;
            EndSurah = endSurah;
            StartVerse = startVerse;
            EndVerse = endVerse;
            Number = number;
        }

        public override string ToString()
        {
            int juz;

            // Find Juz with even test
            if (Number % 2 == 0)
            {
                juz = Number / 2;    
            }
            else
            {
                juz = (Number + 1) / 2;
            }

            return $"Juz {juz}, Hizb {Number}";
        }

        public bool Equals(HizbDescription other)
        {
            return Number == other.Number;
        }
    }
}
