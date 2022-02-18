using Baraka.Services.Quran;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models.Quran
{
    public class SuraModel : IEquatable<SuraModel>
    {
        public int Number { get; } // Starts from one
        public int Length { get; }
        public string PhoneticName { get; }
        public string TranslatedName { get; }
        public SuraRevelationType RevelationType { get; }

        public SuraModel(
            int number,
            int length,
            string phoneticName,
            string translatedName, // TODO: temporary value
            SuraRevelationType revelationType)
        {
            Number = number;
            Length = length;
            PhoneticName = phoneticName;
            TranslatedName = translatedName;
            RevelationType = revelationType;
        }

        public SuraModel Next()
        {
            if (Number < 1 || Number > 113) throw new InvalidOperationException();
            return SuraInfoService.FromNumber(Number + 1);
        }

        public SuraModel Last()
        {
            if (Number < 2 || Number > 114) throw new InvalidOperationException();
            return SuraInfoService.FromNumber(Number - 1);
        }

        public bool Equals(SuraModel other)
        {
            return Number == other.Number;
        }
    }
}
