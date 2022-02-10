using Baraka.Services.Quran;
using System;

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
            if (Number >= SuraInfoService.FromNumber(Sura).Length)
            {
                return new VerseLocationModel(Sura + 1, 1);
            }
            else
            {
                return new VerseLocationModel(Sura, Number + 1);
            }
        }

        public static VerseLocationModel From(SuraModel sura, int verse)
        {
            return new VerseLocationModel(sura.Number, verse);
        }

        public bool IsLast()
        {
            return Sura == 114 && Number == 6;
        }

        public bool Equals(VerseLocationModel other)
        {
            return Sura == other.Sura && Number == other.Number;
        }
    }
}
