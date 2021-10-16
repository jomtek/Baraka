using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models
{
    public class SurahModel
    {
        public readonly int Number;
        public readonly int Length;
        public readonly string ArabicName;
        public readonly string PhoneticName;
        public readonly SurahRevelationType RevelationType;

        public SurahModel(
            int number,
            int length,
            string arabicName, string phoneticName,
            SurahRevelationType revelationType)
        {
            Number = number;
            Length = length;
            ArabicName = arabicName;
            PhoneticName = phoneticName;
            RevelationType = revelationType;
        }
    }
}
