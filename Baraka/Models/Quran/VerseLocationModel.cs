using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models.Quran
{
    [Serializable]
    public struct VerseLocationModel
    {
        public int Sura { get; }
        public int Number { get; }

        public VerseLocationModel(int sura, int number)
        {
            Sura = sura;
            Number = number;
        }
    }
}
