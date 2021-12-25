using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models.Quran
{
    [Serializable]
    public struct VerseLocationModel : IEquatable<VerseLocationModel>
    {
        public int Sura { get; }
        public int Number { get; }

        public VerseLocationModel(int sura, int number)
        {
            Sura = sura;
            Number = number;
        }

        public VerseLocationModel Next()
        {
            return new VerseLocationModel(Sura, Number + 1);
        }

        public static VerseLocationModel From(SuraModel sura, int verse)
        {
            return new VerseLocationModel(sura.Number, verse);
        }

        public bool Equals(VerseLocationModel other)
        {
            return Sura == other.Sura && Number == other.Number;
        }
    }
}
