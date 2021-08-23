using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data.Descriptions
{
    [Serializable]
    public class VerseDescription
    {
        public SurahDescription Surah { get; set; }
        public int Number { get; set; }
        public string ArabicText
        {
            get
            {
                // Works with verse numbers starting from 1
                return LoadedData.SurahList[Surah]["ARABIC"].Verses[Number - 1];
            }
        }

        public VerseDescription(SurahDescription surah, int number)
        {
            Surah = surah;
            Number = number;
        }

        // Works with verse numbers starting from 1
        public VerseDescription Next()
        {
            if (Number < Surah.NumberOfVerses)
            {
                return new VerseDescription(Surah, Number + 1);
            }
            else
            {
                return new VerseDescription(Surah.Next(), 1);
            }
        }

        #region Comparison methods
        public override bool Equals(object obj)
        {
            VerseDescription target = obj as VerseDescription;
            if (target == null)
            {
                return false;
            }
            else
            {
                return Surah.SurahNumber == target.Surah.SurahNumber &&
                       Number == target.Number;
            }
        }

        public override int GetHashCode()
        {
            return (Surah.SurahNumber, Number).GetHashCode();
        }
        #endregion
    }
}
